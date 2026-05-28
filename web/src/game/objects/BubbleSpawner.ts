import Phaser from 'phaser'
import { Bubble } from './Bubble'
import { AUDIO } from '../constants/assets'
import type { SoundController } from '../systems/SoundController'
import { useSettingsStore } from '../../store/settings'

// Mirrors BubblesDup.cs defaults
const BUBBLE_GRAVITY_MIN = 80   // px/s float speed (slow drift)
const BUBBLE_GRAVITY_MAX = 700  // px/s float speed (fast rise)
// Sprite is 394px; this base scale renders bubbles at 16–47px (0.5–1.5× variation)
const BUBBLE_BASE_SCALE = 0.08

export class BubbleSpawner {
  private scene: Phaser.Scene
  private snd: SoundController
  private spawnTimer = 0
  private shakeTimer = 0
  private isShaking = false
  private shakeTimerInterval = 0.13  // shakeBubbleTimer default

  // Active bubble list — filter destroyed ones each frame
  readonly bubbles: Bubble[] = []

  // Spawn parameters from settings (set in constructor)
  private spawnMin: number
  private spawnMax: number
  private shakeCount: number
  private scaleMin: number
  private scaleMax: number
  private baseScale: number

  constructor(scene: Phaser.Scene, snd: SoundController) {
    this.scene = scene
    this.snd = snd

    const s = useSettingsStore().settings
    const freq = s.bubbleFrequency      // 1–8, default 4
    this.spawnMin = Math.max(0.5, freq - 2)
    this.spawnMax = freq + 2
    this.shakeTimerInterval = s.shakenBubbleFrequency
    this.shakeCount = s.bubbleCount
    const variation = s.bubbleSizeVariation
    this.scaleMin = 1 - variation
    this.scaleMax = 1 + variation
    this.baseScale = s.bubbleSize

    this.spawnTimer = 1.0  // first bubble after 1 s so player sees them quickly
  }

  update(delta: number) {
    const dt = delta / 1000

    // Remove destroyed bubbles from tracking array
    for (let i = this.bubbles.length - 1; i >= 0; i--) {
      if (!this.bubbles[i].active) this.bubbles.splice(i, 1)
    }

    // Update live bubbles
    for (const b of this.bubbles) b.bubbleUpdate(delta)

    if (this.isShaking) {
      this.shakeTimer -= dt
      if (this.shakeTimer <= 0) {
        this.spawnBatch(this.shakeCount)
        this.shakeTimer = this.shakeTimerInterval
      }
    } else {
      this.spawnTimer -= dt
      if (this.spawnTimer <= 0) {
        this.spawnBatch(1)
        this.spawnTimer = Phaser.Math.FloatBetween(this.spawnMin, this.spawnMax)
      }
    }
  }

  private spawnBatch(count: number) {
    const gW = this.scene.scale.width
    const gH = this.scene.scale.height
    for (let i = 0; i < count; i++) {
      const x = Phaser.Math.FloatBetween(40, gW - 40)
      const y = gH + 20                               // just below screen bottom
      const scale = Phaser.Math.FloatBetween(this.scaleMin, this.scaleMax) * this.baseScale * BUBBLE_BASE_SCALE
      const floatSpeed = Phaser.Math.FloatBetween(BUBBLE_GRAVITY_MIN, BUBBLE_GRAVITY_MAX)
      const bubble = new Bubble(this.scene, x, y, this.snd, scale, floatSpeed)
      this.bubbles.push(bubble)
    }
  }

  // Shake interface — mirrors BubblesDup.cs
  startShake(_ax: number, _ay: number, _forceMult: number) {
    this.isShaking = true
    this.shakeTimer = this.shakeTimerInterval
    this.snd.play(AUDIO.SHAKE_LIGHT, 1.0, 1.0, true)
  }

  continueShake(_ax: number, _ay: number, _forceMult: number) {
    this.shakeTimer = this.shakeTimerInterval
    this.snd.playRandom(
      [AUDIO.SHAKE_MEDIUM, AUDIO.SHAKE_MEDIUM_2, AUDIO.SHAKE_QUICK],
      1.0
    )
  }

  endShake() {
    this.isShaking = false
  }
}
