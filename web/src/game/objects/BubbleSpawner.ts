import Phaser from 'phaser'
import { Bubble } from './Bubble'
import { AUDIO } from '../constants/assets'
import type { SoundController } from '../systems/SoundController'
import { useSettingsStore } from '../../store/settings'

const BUBBLE_GRAVITY_MIN = 80    // px/s float speed (slow drift)
const BUBBLE_GRAVITY_MAX = 350   // px/s (was 700 — too fast to see)
// Sprite 394px: at max settings (bubbleSize=3, variation=1) → largest ≈ 0.22*6*394 ≈ 520px (full screen)
const BUBBLE_BASE_SCALE = 0.22
const MAX_BUBBLES = 30  // cap prevents bubble pile-up from blocking fish taps

export class BubbleSpawner {
  private scene: Phaser.Scene
  private snd: SoundController
  private spawnTimer = 0
  private isShaking = false
  private shakeBurstTimer = 0   // cooldown between secondary bursts during a shake
  private shakeIntensity = 1    // magnitude of latest acceleration reading

  readonly bubbles: Bubble[] = []

  private spawnMin: number
  private spawnMax: number
  private shakeIntensityMultiplier: number  // from settings bubbleCount (1–10)
  private scaleMin: number
  private scaleMax: number
  private baseScale: number

  constructor(scene: Phaser.Scene, snd: SoundController) {
    this.scene = scene
    this.snd = snd

    const s = useSettingsStore().settings
    const freq = s.bubbleFrequency
    // Higher freq value = more bubbles (shorter interval). interval = 10/freq.
    const interval = 10 / freq
    this.spawnMin = Math.max(0.4, interval * 0.6)
    this.spawnMax = interval * 1.4
    this.shakeIntensityMultiplier = s.bubbleCount  // 1–10 → scales burst size
    const variation = s.bubbleSizeVariation
    this.scaleMin = 1 - variation
    this.scaleMax = 1 + variation
    this.baseScale = s.bubbleSize

    this.spawnBatch(5, true)
    this.spawnTimer = Phaser.Math.FloatBetween(this.spawnMin, this.spawnMax)
  }

  update(delta: number) {
    const dt = delta / 1000

    for (let i = this.bubbles.length - 1; i >= 0; i--) {
      if (!this.bubbles[i].active) this.bubbles.splice(i, 1)
    }
    for (const b of this.bubbles) b.bubbleUpdate(delta)

    if (this.isShaking) {
      // Secondary bursts while shaking — smaller and slower than the initial blast
      this.shakeBurstTimer -= dt
      if (this.shakeBurstTimer <= 0) {
        const count = Math.round(this.shakeIntensityMultiplier * this.shakeIntensity * 0.6)
        this.spawnBatch(Phaser.Math.Clamp(count, 1, 8), false)
        this.shakeBurstTimer = 0.55
      }
    } else {
      this.spawnTimer -= dt
      if (this.spawnTimer <= 0) {
        this.spawnBatch(2, false)
        this.spawnTimer = Phaser.Math.FloatBetween(this.spawnMin, this.spawnMax)
      }
    }
  }

  private spawnBatch(count: number, scattered = false) {
    const gW = this.scene.scale.width
    const gH = this.scene.scale.height
    for (let i = 0; i < count; i++) {
      if (this.bubbles.length >= MAX_BUBBLES) break
      const x = Phaser.Math.FloatBetween(40, gW - 40)
      const y = scattered
        ? Phaser.Math.FloatBetween(gH * 0.3, gH * 0.95)
        : gH + 20
      const scale = Phaser.Math.FloatBetween(this.scaleMin, this.scaleMax) * this.baseScale * BUBBLE_BASE_SCALE
      const floatSpeed = Phaser.Math.FloatBetween(BUBBLE_GRAVITY_MIN, BUBBLE_GRAVITY_MAX)
      this.bubbles.push(new Bubble(this.scene, x, y, this.snd, scale, floatSpeed))
    }
  }

  startShake(ax: number, ay: number, _forceMult: number) {
    this.isShaking = true
    this.shakeIntensity = Math.sqrt(ax * ax + ay * ay)
    // Big immediate burst — proportional to shake strength and user intensity setting
    const burst = Math.round(this.shakeIntensityMultiplier * this.shakeIntensity * 2)
    this.spawnBatch(Phaser.Math.Clamp(burst, this.shakeIntensityMultiplier, this.shakeIntensityMultiplier * 5), false)
    this.shakeBurstTimer = 0.4  // short pause before secondary bursts begin
    this.snd.play(AUDIO.SHAKE_LIGHT, 1.0, 1.0, true)
  }

  continueShake(ax: number, ay: number, _forceMult: number) {
    this.shakeIntensity = Math.max(this.shakeIntensity, Math.sqrt(ax * ax + ay * ay))
    this.snd.playRandom(
      [AUDIO.SHAKE_MEDIUM, AUDIO.SHAKE_MEDIUM_2, AUDIO.SHAKE_QUICK],
      1.0
    )
  }

  endShake() {
    this.isShaking = false
  }
}
