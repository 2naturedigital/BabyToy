import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM } from '../constants/assets'

const WOBBLE_SHAKE_SPEED = 300  // deg/s while shaking / post-shake
const POST_SHAKE_SECS    = 2.5  // deceleration window after shake ends

export class Starfish extends FishController {
  private wobbleAngle = 0
  private rotDir = 1
  private reacting = false
  private postShakeTimer = 0

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'starfish_4', 50, 160)
    this.baseScale = 0.226
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    // Play the frame animation so eyebrow/mouth expressions cycle naturally
    this.play(ANIM.STARFISH_WOBBLE)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.stop()  // pause frame animation during spin
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
              this.setAngle(0)
              this.wobbleAngle = 0
              this.reacting = false
              this.play(ANIM.STARFISH_WOBBLE)  // resume expression animation
            },
          })
        },
      })
    })
  }

  // Stop the frame animation when shaking starts so we can take over with setAngle
  override startShake(ax: number, ay: number, forceMult: number) {
    super.startShake(ax, ay, forceMult)
    this.stop()
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
      // Decelerate from WOBBLE_SHAKE_SPEED down to 0 over POST_SHAKE_SECS
      const t = Math.max(0, this.postShakeTimer / POST_SHAKE_SECS)
      const speed = WOBBLE_SHAKE_SPEED * t
      this.wobbleAngle += this.rotDir * speed * dt
      if (Math.abs(this.wobbleAngle) > 360) {
        this.wobbleAngle = ((this.wobbleAngle % 360) + 360) % 360
        if (this.wobbleAngle > 180) this.wobbleAngle -= 360
      }
      this.setAngle(this.wobbleAngle)
      if (this.postShakeTimer <= 0) {
        // Spin fully wound down — snap upright and let the frame animation take over
        this.setAngle(0)
        this.wobbleAngle = 0
        this.rotDir = 1
        this.play(ANIM.STARFISH_WOBBLE)
      }
    }
    // Normal state: STARFISH_WOBBLE animation is running, no manual setAngle needed
  }
}
