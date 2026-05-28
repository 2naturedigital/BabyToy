import Phaser from 'phaser'
import type { SoundController } from '../systems/SoundController'

export abstract class FishController extends Phaser.GameObjects.Sprite {
  protected readonly gameW: number
  protected readonly gameH: number

  // Movement
  protected minSpeed: number
  protected maxSpeed: number
  protected targetX = 0
  protected targetY = 0
  protected isFacingLeft = true

  // Half-dimensions for boundary clamping (set in init)
  protected halfW = 0
  protected halfH = 0

  // Shake state — mirrors FishController.cs fields
  protected isShaking = false
  protected isResetTime = false
  protected magnitudeMult = 1      // acceleration sqrMagnitude, multiplies speed
  protected shakeForceMultiplier = 1

  protected snd!: SoundController

  constructor(scene: Phaser.Scene, x: number, y: number, texture: string, minSpeed: number, maxSpeed: number) {
    super(scene, x, y, texture)
    scene.add.existing(this)
    this.minSpeed = minSpeed
    this.maxSpeed = maxSpeed
    this.gameW = scene.scale.width
    this.gameH = scene.scale.height
  }

  /** Call once after construction with the user-chosen size multiplier. */
  init(snd: SoundController, size: number) {
    this.snd = snd
    this.setScale(size)
    this.halfW = this.displayWidth / 2
    this.halfH = this.displayHeight / 2
    this.setInteractive()
    this.x = Phaser.Math.FloatBetween(this.halfW, this.gameW - this.halfW)
    this.y = Phaser.Math.FloatBetween(this.halfH, this.gameH - this.halfH)
    this.setRandomTarget()
  }

  protected setRandomTarget() {
    this.targetX = Phaser.Math.FloatBetween(this.halfW, this.gameW - this.halfW)
    this.targetY = Phaser.Math.FloatBetween(this.halfH, this.gameH - this.halfH)
  }

  // Mirrors Vector3.MoveTowards — direct position, no physics.
  protected moveFish(delta: number) {
    const speed = Phaser.Math.FloatBetween(this.minSpeed, this.maxSpeed) * this.magnitudeMult
    const dx = this.targetX - this.x
    const dy = this.targetY - this.y
    const dist = Math.sqrt(dx * dx + dy * dy)

    if (dist > 4) {
      if (dx < 0 && !this.isFacingLeft) this.flipHorizontal()
      else if (dx > 0 && this.isFacingLeft) this.flipHorizontal()

      const step = Math.min(speed * delta / 1000, dist)
      this.x += (dx / dist) * step
      this.y += (dy / dist) * step
    } else {
      this.setRandomTarget()
    }
  }

  protected flipHorizontal() {
    this.isFacingLeft = !this.isFacingLeft
    this.setFlipX(!this.flipX)
  }

  // Shake interface — mirrors FishController.cs public methods
  startShake(ax: number, ay: number, forceMult: number) {
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
    this.isShaking = true
    this.isResetTime = false
  }

  continueShake(ax: number, ay: number, forceMult: number) {
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
  }

  endShake() {
    this.magnitudeMult = 1
    this.shakeForceMultiplier = 1
    this.isShaking = false
    this.isResetTime = true
  }

  abstract fishUpdate(delta: number): void
}
