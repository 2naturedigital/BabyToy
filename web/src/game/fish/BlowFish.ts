import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM, AUDIO } from '../constants/assets'

// Physics tuned for 540×960 canvas — gentle floating motion
const PUMP_MIN_TIME = 2.0      // seconds between pumps (longer = calmer)
const PUMP_MAX_TIME = 6.0
const PUMP_POWER_MIN = 60      // px/s upward impulse (was 200 — way too strong)
const PUMP_POWER_MAX = 180     // max travel ~270px upward (180²/2/60)
const BODY_GRAVITY = 60        // px/s² downward — gentle sink like floating in water
const MAX_VERT_SPEED = 220     // cap vertical velocity to prevent flying off screen
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
    // Sprite is 440px wide; this renders it at ~198px on the 540px canvas,
    // matching the reference Unity scale of 100.
    this.baseScale = 0.45
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    // Add arcade physics body for gravity + impulse movement
    this.scene.physics.add.existing(this)
    this.body2d = this.body as Phaser.Physics.Arcade.Body
    this.body2d.setGravityY(BODY_GRAVITY)
    // Lock hitbox to the normal (unpuffed) swim body so the inflated visual
    // doesn't create a disproportionately large tap area.  The sprite texture
    // is 440×380; the swim body fills roughly 60% of each axis.
    this.body2d.setSize(Math.round(this.displayWidth * 0.6), Math.round(this.displayHeight * 0.6), true)
    // Also reset the interactive (tap) area to match
    this.setInteractive(
      new Phaser.Geom.Rectangle(88, 76, 264, 228),
      Phaser.Geom.Rectangle.Contains
    )
    this.body2d.setCollideWorldBounds(false)
    this.body2d.setDragX(90)
    this.body2d.setDragY(40)  // increased water resistance to damp vertical overshoot

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

    // Hard cap on vertical speed prevents flying off screen if pump compounds
    if (this.body2d.velocity.y < -MAX_VERT_SPEED) this.body2d.velocity.y = -MAX_VERT_SPEED
    if (this.body2d.velocity.y >  MAX_VERT_SPEED) this.body2d.velocity.y =  MAX_VERT_SPEED

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

  private boundsCheckVertical() {
    const topBound = this.halfH
    const bottomBound = this.gameH - this.halfH

    if (this.y <= topBound) {
      // Hit the ceiling — stop upward motion and wait before pumping again
      this.y = topBound
      if (this.body2d.velocity.y < 0) this.body2d.velocity.y = 0
      if (this.pumpTimer < PUMP_MAX_TIME * 0.6) this.pumpTimer = PUMP_MAX_TIME * 0.6
    } else if (this.y >= bottomBound) {
      // Hit the floor — schedule a pump soon (not instant, avoid jerk)
      this.y = bottomBound
      if (this.body2d.velocity.y > 0) this.body2d.velocity.y = 0
      if (this.pumpTimer > 0.8) this.pumpTimer = Phaser.Math.FloatBetween(0.3, 0.8)
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
