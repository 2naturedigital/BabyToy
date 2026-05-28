import Phaser from 'phaser'
import { FishController } from './FishController'

// Wobble constants (match original inspector values as closely as possible)
const WOBBLE_SPEED = 45        // deg/s during normal swim
const WOBBLE_SHAKE_SPEED = 300 // deg/s while shaking or resetting
const WOBBLE_MAX_DEG = 22      // max rotation angle

export class Starfish extends FishController {
  private wobbleAngle = 0
  private rotDir = 1
  // reacting is also checked in updateWobble — tween controls angle while true
  private reacting = false

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'starfish_4', 50, 160)
    // Sprite is 580px wide; this renders it at ~131px on the 540px canvas,
    // matching the relative Unity scale of 50 (vs Blowfish 100).
    this.baseScale = 0.226
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      const baseScale = this.scaleX
      // Spin 360° + grow, then shrink back with a bounce.
      // updateWobble is paused during reacting so the tween controls angle.
      this.scene.tweens.add({
        targets: this,
        angle: '+=360',
        scaleX: baseScale * 1.45,
        scaleY: baseScale * 1.45,
        duration: 500,
        ease: 'Power2',
        onComplete: () => {
          this.scene.tweens.add({
            targets: this,
            scaleX: baseScale,
            scaleY: baseScale,
            duration: 300,
            ease: Phaser.Math.Easing.Back.Out,
            onComplete: () => {
              // Sync wobble state so there's no angle jump when wobble resumes
              this.wobbleAngle = this.angle
              this.reacting = false
            },
          })
        },
      })
    })
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
    this.updateWobble(delta)
  }

  // Mirrors Starfish.cs AnimateFish() — three states: normal / shaking / resetting
  private updateWobble(delta: number) {
    // Pause wobble while the tap-spin tween is running; it owns the angle
    if (this.reacting) return

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
