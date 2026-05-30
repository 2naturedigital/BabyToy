import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM, AUDIO } from '../constants/assets'

const PUMP_MIN_TIME = 2.0
const PUMP_MAX_TIME = 6.0
const PUMP_POWER_MIN = 60
const PUMP_POWER_MAX = 180
const BODY_GRAVITY = 60
const MAX_VERT_SPEED = 220
const ROTATION_SPEED = 60
const ROTATION_SHAKE_SPEED = 180

// How long (ms) blowfish stays puffed after tap animation completes
const TAP_PUFFED_HOLD_MS   = 1800
// How long (s) blowfish stays inflated after a shake ends
const SHAKE_DEFLATE_DELAY  = 2.5

export class BlowFish extends FishController {
  private body2d!: Phaser.Physics.Arcade.Body
  private pumpTimer = 0
  private rotDir = 1
  private isInflated = false
  private reacting = false
  private deflateTimer = 0   // counts down before deflating after shake ends
  private deflating = false  // true while the shrink animation plays

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'blowfish_swim_1', 0, 0)
    // Blowfish sprite is 440×380; guppy is 318×342 at scale 0.36 ≈ 123px tall.
    // Scale 0.45 → 380*0.45 = 171px normal swim size, bigger than the guppy.
    // Inflated frames are visually larger still.
    this.baseScale = 0.45
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    this.scene.physics.add.existing(this)
    this.body2d = this.body as Phaser.Physics.Arcade.Body
    this.body2d.setGravityY(BODY_GRAVITY)
    this.body2d.setCollideWorldBounds(false)
    this.body2d.setDragX(90)
    this.body2d.setDragY(40)
    // Constrain hitbox to swim-body dimensions so the inflated visual
    // doesn't expand the tap area beyond the actual fish body.
    this.body2d.setSize(Math.round(this.displayWidth * 0.6), Math.round(this.displayHeight * 0.6), true)
    this.setInteractive(
      new Phaser.Geom.Rectangle(88, 76, 264, 228),
      Phaser.Geom.Rectangle.Contains
    )

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.play(ANIM.BLOWFISH_INFLATE)
      this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
        // Hold fully-puffed then animate back down through the shrink frames
        this.scene.time.delayedCall(TAP_PUFFED_HOLD_MS, () => {
          if (!this.active) return
          if (!this.isInflated && !this.deflating) {
            this.shrinkBack(() => { this.reacting = false })
          } else {
            this.reacting = false  // shake has taken over inflation state
          }
        })
      })
    })

    this.play(ANIM.BLOWFISH_SWIM)
    this.pumpTimer = Phaser.Math.FloatBetween(PUMP_MIN_TIME, PUMP_MAX_TIME)
  }

  fishUpdate(delta: number) {
    const dt = delta / 1000

    if (this.body2d.velocity.y < -MAX_VERT_SPEED) this.body2d.velocity.y = -MAX_VERT_SPEED
    if (this.body2d.velocity.y >  MAX_VERT_SPEED) this.body2d.velocity.y =  MAX_VERT_SPEED

    this.boundsCheckVertical()
    this.boundsCheckHorizontal()

    if (!this.isShaking) {
      if (this.isResetTime) {
        this.addRotation(ROTATION_SHAKE_SPEED + 30, dt)
        if (Math.abs(this.angle) < 3) {
          this.setAngle(0)
          this.isResetTime = false
        }
      }

      this.pumpTimer -= dt
      if (this.pumpTimer <= 0) {
        this.pump()
        if (!this.isInflated && !this.isResetTime) this.rotDir *= -1
        this.pumpTimer = Phaser.Math.FloatBetween(PUMP_MIN_TIME, PUMP_MAX_TIME)
      }
    } else {
      this.addRotation(ROTATION_SHAKE_SPEED, dt)
      // Reset deflate timer each frame while still shaking
      this.deflateTimer = SHAKE_DEFLATE_DELAY
    }

    // Inflate on shake start
    if (this.isShaking && !this.isInflated) {
      this.isInflated = true
      this.deflating = false  // cancel any in-progress shrink
      this.deflateTimer = SHAKE_DEFLATE_DELAY
      this.snd.play(AUDIO.BF_INFLATE)
      this.play(ANIM.BLOWFISH_INFLATE)
    }

    // After shake ends, count down then animate back through the shrink frames
    if (!this.isShaking && this.isInflated && !this.deflating) {
      this.deflateTimer -= dt
      if (this.deflateTimer <= 0) {
        this.isInflated = false
        this.snd.play(AUDIO.BF_DEFLATE)
        this.shrinkBack(null)
      }
    }
  }

  // Play the deflate animation then return to swim. Optional callback fires after.
  private shrinkBack(onDone: (() => void) | null) {
    this.deflating = true
    this.play(ANIM.BLOWFISH_SHRINK)
    this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
      this.deflating = false
      if (!this.active || this.isInflated) return
      this.play(ANIM.BLOWFISH_SWIM)
      onDone?.()
    })
  }

  private pump() {
    const power = Phaser.Math.FloatBetween(PUMP_POWER_MIN, PUMP_POWER_MAX)
    this.body2d.velocity.y -= power
    this.snd.playRandom(
      ['blowfish_swim_sfx_1','blowfish_swim_sfx_2','blowfish_swim_sfx_3',
       'blowfish_swim_sfx_4','blowfish_swim_sfx_5','blowfish_swim_sfx_6'],
      Phaser.Math.FloatBetween(0.6, 0.9)
    )
  }

  private addRotation(speed: number, dt: number) {
    this.setAngle(this.angle + this.rotDir * speed * dt)
  }

  private boundsCheckVertical() {
    const topBound    = this.halfH
    const bottomBound = this.gameH - this.halfH

    if (this.y <= topBound) {
      this.y = topBound
      if (this.body2d.velocity.y < 0) this.body2d.velocity.y = 0
      if (this.pumpTimer < PUMP_MAX_TIME * 0.6) this.pumpTimer = PUMP_MAX_TIME * 0.6
    } else if (this.y >= bottomBound) {
      this.y = bottomBound
      if (this.body2d.velocity.y > 0) this.body2d.velocity.y = 0
      if (this.pumpTimer > 0.8) this.pumpTimer = Phaser.Math.FloatBetween(0.3, 0.8)
    }
  }

  private boundsCheckHorizontal() {
    const edgeW = this.isInflated ? this.halfW : this.halfW / 2
    if (this.x < -edgeW) {
      this.x = this.gameW + edgeW
    } else if (this.x > this.gameW + edgeW) {
      this.x = -edgeW
    }
  }

  applyForce(vx: number) {
    this.body2d.velocity.x += vx
  }
}
