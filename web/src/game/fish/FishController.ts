import Phaser from 'phaser'
import type { SoundController } from '../systems/SoundController'

export abstract class FishController extends Phaser.GameObjects.Sprite {
  protected readonly gameW: number
  protected readonly gameH: number

  protected minSpeed: number
  protected maxSpeed: number
  protected currentSpeed = 0
  protected targetX = 0
  protected targetY = 0
  protected isFacingLeft = true

  protected halfW = 0
  protected halfH = 0

  protected isShaking = false
  protected isResetTime = false
  protected magnitudeMult = 1
  protected shakeForceMultiplier = 1

  private magnitudeWindRate = 0  // how fast magnitudeMult decays back to 1 after shake

  protected snd!: SoundController
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
    this.currentSpeed = Phaser.Math.FloatBetween(this.minSpeed, this.maxSpeed)
  }

  protected moveFish(delta: number) {
    // Gradually wind down speed boost over 2 seconds after shake ends
    if (!this.isShaking && this.magnitudeWindRate > 0) {
      this.magnitudeMult -= this.magnitudeWindRate * (delta / 1000)
      if (this.magnitudeMult <= 1.0) {
        this.magnitudeMult = 1.0
        this.magnitudeWindRate = 0
      }
    }

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

  startShake(_ax: number, _ay: number, _forceMult: number) {
    // Fixed modest speed boost — not proportional to raw acceleration magnitude
    this.magnitudeWindRate = 0
    this.magnitudeMult = 1.5
    this.shakeForceMultiplier = 1
    this.isShaking = true
    this.isResetTime = false
  }

  continueShake(_ax: number, _ay: number, _forceMult: number) {
    // magnitudeMult stays at 1.5 for duration of shake
  }

  endShake() {
    this.isShaking = false
    this.isResetTime = true
    this.shakeForceMultiplier = 1
    // Wind down over 2 seconds instead of snapping back instantly
    this.magnitudeWindRate = (this.magnitudeMult - 1.0) / 2.0
  }

  abstract fishUpdate(delta: number): void
}
