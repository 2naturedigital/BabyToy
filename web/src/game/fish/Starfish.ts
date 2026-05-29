import Phaser from 'phaser'
import { FishController } from './FishController'

const WOBBLE_SPEED       = 45   // deg/s normal swim
const WOBBLE_SHAKE_SPEED = 300  // deg/s while shaking
const WOBBLE_MAX_DEG     = 22   // normal wobble range
const POST_SHAKE_SECS    = 2.5  // how long to decelerate after shake ends

export class Starfish extends FishController {
  private wobbleAngle = 0
  private rotDir = 1
  private reacting = false       // true while tap-spin tween runs
  private postShakeTimer = 0     // counts down after shake; spins at decelerating speed

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'starfish_4', 50, 160)
    this.baseScale = 0.226
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      const baseScale = this.scaleX
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
              this.wobbleAngle = this.angle
              this.reacting = false
            },
          })
        },
      })
    })
  }

  override endShake() {
    super.endShake()
    // Normalise accumulated angle to [-180, 180] so the decel loop terminates
    let a = ((this.wobbleAngle % 360) + 360) % 360
    if (a > 180) a -= 360
    this.wobbleAngle = a
    // Keep spinning in the same direction — deceleration handles the slow-down
    this.postShakeTimer = POST_SHAKE_SECS
    this.isResetTime = false  // handled by postShakeTimer now
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
    this.updateWobble(delta)
  }

  private updateWobble(delta: number) {
    if (this.reacting) return

    const dt = delta / 1000

    if (this.isShaking) {
      this.wobbleAngle += this.rotDir * WOBBLE_SHAKE_SPEED * dt
    } else if (this.postShakeTimer > 0) {
      this.postShakeTimer -= dt
      // Linearly decelerate from WOBBLE_SHAKE_SPEED down to WOBBLE_SPEED
      const t = Math.max(0, this.postShakeTimer / POST_SHAKE_SECS)  // 1→0
      const speed = WOBBLE_SPEED + (WOBBLE_SHAKE_SPEED - WOBBLE_SPEED) * t
      this.wobbleAngle += this.rotDir * speed * dt
      // Keep angle from accumulating beyond one full rotation
      if (Math.abs(this.wobbleAngle) > 360) {
        this.wobbleAngle = ((this.wobbleAngle % 360) + 360) % 360
        if (this.wobbleAngle > 180) this.wobbleAngle -= 360
      }
      // Snap cleanly into normal wobble once the timer expires
      if (this.postShakeTimer <= 0) {
        this.wobbleAngle = Phaser.Math.Clamp(this.wobbleAngle, -WOBBLE_MAX_DEG, WOBBLE_MAX_DEG)
        this.rotDir = 1
      }
    } else {
      // Normal gentle wobble
      this.wobbleAngle += this.rotDir * WOBBLE_SPEED * dt
      if (Math.abs(this.wobbleAngle) >= WOBBLE_MAX_DEG) this.rotDir *= -1
    }

    this.setAngle(this.wobbleAngle)
  }
}
