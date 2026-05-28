import Phaser from 'phaser'
import type { SoundController } from '../systems/SoundController'

export abstract class FishController extends Phaser.GameObjects.Sprite {
  protected readonly gameW: number
  protected readonly gameH: number

  protected minSpeed: number
  protected maxSpeed: number
  protected currentSpeed = 0   // fixed for each journey, not randomised per-frame
  protected targetX = 0
  protected targetY = 0
  protected isFacingLeft = true

  protected halfW = 0
  protected halfH = 0

  protected isShaking = false
  protected isResetTime = false
  protected magnitudeMult = 1
  protected shakeForceMultiplier = 1

  protected snd!: SoundController
  // Intrinsic scale calibrated so that size=1.0 matches the original Unity appearance.
  // Each subclass sets this in its constructor.
  protected baseScale = 1.0

  constructor(scene: Phaser.Scene, x: number, y: number, texture: string, minSpeed: number, maxSpeed: number) {
    super(scene, x, y, texture)
    scene.add.existing(this)
    this.minSpeed = minSpeed
    this.maxSpeed = maxSpeed
    this.gameW = scene.scale.width
    this.gameH = scene.scale.height
  }

  init(snd: SoundController, size: number) {
    this.snd = snd
    this.setScale(this.baseScale * size)
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
    // Pick speed once per journey — eliminates per-frame jitter
    this.currentSpeed = Phaser.Math.FloatBetween(this.minSpeed, this.maxSpeed)
  }

  protected moveFish(delta: number) {
    const speed = this.currentSpeed * this.magnitudeMult
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
