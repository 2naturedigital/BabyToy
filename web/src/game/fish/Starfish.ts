import Phaser from 'phaser'
import { FishController } from './FishController'

// Wobble constants (match original inspector values as closely as possible)
const WOBBLE_SPEED = 45        // deg/s during normal swim
const WOBBLE_SHAKE_SPEED = 300 // deg/s while shaking or resetting
const WOBBLE_MAX_DEG = 22      // max rotation angle

export class Starfish extends FishController {
  private wobbleAngle = 0
  private rotDir = 1
  private reacting = false

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'starfish_4', 50, 160)
    // Sprite is 580px wide; this renders it at ~131px on the 540px canvas,
    // matching the relative Unity scale of 50 (vs Blowfish 100).
    this.baseScale = 0.226
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    // Tap → quick scale pulse (Unity played a "bigeye" anim we recreate as a tween)
    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      const base = this.scaleX
      this.scene.tweens.add({
        targets: this,
        scaleX: base * 1.35,
        scaleY: base * 1.35,
        duration: 180,
        yoyo: true,
        onComplete: () => { this.reacting = false }
      })
    })
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
    this.updateWobble(delta)
  }

  // Mirrors Starfish.cs AnimateFish() — three states: normal / shaking / resetting
  private updateWobble(delta: number) {
    const dt = delta / 1000

    if (!this.isShaking && !this.isResetTime) {
      this.wobbleAngle += this.rotDir * WOBBLE_SPEED * dt
      if (Math.abs(this.wobbleAngle) >= WOBBLE_MAX_DEG) {
        this.rotDir *= -1
      }
    } else if (this.isShaking) {
      this.wobbleAngle += this.rotDir * WOBBLE_SHAKE_SPEED * dt
    } else if (this.isResetTime) {
      // Spin back toward 0 at shake speed, then stop
      this.wobbleAngle += this.rotDir * WOBBLE_SHAKE_SPEED * dt
      if (Math.abs(this.wobbleAngle) <= WOBBLE_MAX_DEG) {
        this.isResetTime = false
      }
    }

    this.setAngle(this.wobbleAngle)
  }
}
