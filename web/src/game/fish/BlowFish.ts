import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM, AUDIO } from '../constants/assets'

// Inspector values translated to px (canvas 1080×1920)
const PUMP_MIN_TIME = 0.3      // seconds between pumps
const PUMP_MAX_TIME = 3.0
const PUMP_POWER_MIN = 200     // px/s upward impulse
const PUMP_POWER_MAX = 550
const BODY_GRAVITY = 260       // px/s² downward
const ROTATION_SPEED = 60      // deg/s normal rotation
const ROTATION_SHAKE_SPEED = 180

export class BlowFish extends FishController {
  private body2d!: Phaser.Physics.Arcade.Body
  private pumpTimer = 0
  private rotDir = 1
  private isInflated = false
  private reacting = false

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'blowfish_swim_1', 0, 0) // no MoveTowards — physics-driven
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    // Add arcade physics body for gravity + impulse movement
    this.scene.physics.add.existing(this)
    this.body2d = this.body as Phaser.Physics.Arcade.Body
    this.body2d.setGravityY(BODY_GRAVITY)
    this.body2d.setCollideWorldBounds(false)
    // Water resistance — prevents runaway horizontal velocity from tank current
    this.body2d.setDragX(90)
    this.body2d.setDragY(20)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.play(ANIM.BLOWFISH_INFLATE)
      this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
        this.reacting = false
        if (!this.isInflated) this.play(ANIM.BLOWFISH_SWIM)
      })
    })

    this.play(ANIM.BLOWFISH_SWIM)
    this.pumpTimer = Phaser.Math.FloatBetween(PUMP_MIN_TIME, PUMP_MAX_TIME)
  }

  fishUpdate(delta: number) {
    const dt = delta / 1000

    this.boundsCheckVertical()
    this.boundsCheckHorizontal()

    if (!this.isShaking) {
      // Head back to upright after shake
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
        if (!this.isInflated && !this.isResetTime) {
          this.rotDir *= -1
        }
        this.pumpTimer = Phaser.Math.FloatBetween(PUMP_MIN_TIME, PUMP_MAX_TIME)
      }
    } else {
      // Spin while shaking
      this.addRotation(ROTATION_SHAKE_SPEED, dt)
    }

    // Inflate / deflate sounds on state transition — mirrors BlowFish.cs FixedUpdate
    if (this.isShaking && !this.isInflated) {
      this.isInflated = true
      this.snd.play(AUDIO.BF_INFLATE)
      this.play(ANIM.BLOWFISH_INFLATE)
    }
    if (!this.isShaking && this.isInflated) {
      this.isInflated = false
      this.snd.play(AUDIO.BF_DEFLATE)
      this.play(ANIM.BLOWFISH_SWIM)
    }
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

  // Mirrors BlowFish.cs PositionCheckVertical
  private boundsCheckVertical() {
    if (this.y < this.halfH) {
      // Near top — stop pumping for a while
      this.pumpTimer = PUMP_MAX_TIME
      if (this.y <= -this.halfH) {
        this.y = Phaser.Math.Clamp(this.y, -this.halfH, this.halfH)
      }
    } else if (this.y >= this.gameH - this.halfH) {
      if (this.isShaking) {
        // Bounce off bottom while inflated
        this.y = Phaser.Math.Clamp(this.y, this.gameH - this.halfH, this.gameH + this.halfH)
        this.body2d.velocity.y *= -1
      } else {
        // Pump immediately at bottom
        this.pumpTimer = 0
      }
    }
  }

  // Mirrors BlowFish.cs PositionCheckHorizontal — wrap screen edges
  private boundsCheckHorizontal() {
    const edgeW = this.isInflated ? this.halfW : this.halfW / 2
    if (this.x < -edgeW) {
      this.x = this.gameW + edgeW
    } else if (this.x > this.gameW + edgeW) {
      this.x = -edgeW
    }
  }

  // TankCurrent calls this to apply horizontal force each frame
  applyForce(vx: number) {
    this.body2d.velocity.x += vx
  }
}
