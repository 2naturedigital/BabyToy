import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM } from '../constants/assets'

const WOBBLE_SHAKE_SPEED = 300  // deg/s while shaking / post-shake
const POST_SHAKE_SECS    = 2.5  // deceleration window after shake ends
const IDLE_SWAY_DEG      = 14   // gentle body rock on top of blink animation
const IDLE_SWAY_SPEED    = 38   // deg/s

export class Starfish extends FishController {
  private wobbleAngle = 0
  private rotDir = 1
  private reacting = false
  private postShakeTimer = 0

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'starfish_5', 50, 160)  // start on the upright big-eye frame
    this.baseScale = 0.226
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    this.play(ANIM.STARFISH_BLINK)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.stop()
      this.setTexture('starfish_5')  // jump straight to wide-eyes upright frame
      const baseScale = this.scaleX
      this.scene.tweens.add({
        targets: this,
        angle: '+=360',
        scaleX: baseScale * 1.4,
        scaleY: baseScale * 1.4,
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
              this.setAngle(0)
              this.wobbleAngle = 0
              // Hold wide-eyed a beat then animate back to normal
              this.scene.time.delayedCall(500, () => {
                if (!this.active || this.isShaking) { this.reacting = false; return }
                this.recoverThenBlink()
              })
            },
          })
        },
      })
    })
  }

  override startShake(ax: number, ay: number, forceMult: number) {
    super.startShake(ax, ay, forceMult)
    this.stop()
    this.setTexture('starfish_5')  // wide eyes during shake spin
  }

  override endShake() {
    super.endShake()
    let a = ((this.wobbleAngle % 360) + 360) % 360
    if (a > 180) a -= 360
    this.wobbleAngle = a
    this.postShakeTimer = POST_SHAKE_SECS
    this.isResetTime = false
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
    this.updateSpin(delta)
  }

  private updateSpin(delta: number) {
    if (this.reacting) return

    const dt = delta / 1000

    if (this.isShaking) {
      this.wobbleAngle += this.rotDir * WOBBLE_SHAKE_SPEED * dt
      this.setAngle(this.wobbleAngle)
    } else if (this.postShakeTimer > 0) {
      this.postShakeTimer -= dt
      const t = Math.max(0, this.postShakeTimer / POST_SHAKE_SECS)
      this.wobbleAngle += this.rotDir * WOBBLE_SHAKE_SPEED * t * dt
      if (Math.abs(this.wobbleAngle) > 360) {
        this.wobbleAngle = ((this.wobbleAngle % 360) + 360) % 360
        if (this.wobbleAngle > 180) this.wobbleAngle -= 360
      }
      this.setAngle(this.wobbleAngle)
      if (this.postShakeTimer <= 0) {
        this.setAngle(0)
        this.wobbleAngle = 0
        this.rotDir = 1
        this.recoverThenBlink()
      }
    } else {
      // Gentle body sway on top of the blink animation
      this.wobbleAngle += this.rotDir * IDLE_SWAY_SPEED * dt
      if (Math.abs(this.wobbleAngle) >= IDLE_SWAY_DEG) this.rotDir *= -1
      this.setAngle(this.wobbleAngle)
    }
  }

  // Play the recover animation (eyes returning to normal), then resume idle blink
  private recoverThenBlink() {
    this.reacting = true
    this.play(ANIM.STARFISH_RECOVER)
    this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
      if (!this.active) return
      this.reacting = false
      this.wobbleAngle = 0
      this.rotDir = 1
      if (!this.isShaking) this.play(ANIM.STARFISH_BLINK)
    })
  }
}
