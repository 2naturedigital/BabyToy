import Phaser from 'phaser'
import type { BlowFish } from '../fish/BlowFish'

// Mirrors TankCurrent.cs — applies a slow horizontal drift to BlowFish only.
export class TankCurrent {
  private fish: BlowFish[] = []
  private dirX = 1
  private strength: number
  private magnitudeMult = 1
  private shakeForceMultiplier = 1
  private isShaking = false
  private elapsedTime = 0
  private movementPeriod: number
  private readonly minStr: number
  private readonly maxStr: number
  private readonly minPeriod: number
  private readonly maxPeriod: number

  constructor(minStr: number, maxStr: number, minPeriod: number, maxPeriod: number) {
    this.minStr = minStr
    this.maxStr = maxStr
    this.minPeriod = minPeriod
    this.maxPeriod = maxPeriod
    this.strength = Phaser.Math.FloatBetween(minStr, maxStr)
    this.movementPeriod = Phaser.Math.Between(minPeriod, maxPeriod)
    this.dirX = Math.random() < 0.5 ? -1 : 1
  }

  setFish(fish: BlowFish) {
    this.fish.push(fish)
  }

  update(delta: number) {
    const dt = delta / 1000

    this.elapsedTime += dt
    if (this.elapsedTime > this.movementPeriod) {
      this.elapsedTime = 0
      if (!this.isShaking) {
        this.dirX *= -1
      }
      this.strength = Phaser.Math.FloatBetween(
        this.minStr * this.magnitudeMult,
        this.maxStr * this.magnitudeMult
      )
      this.movementPeriod = Phaser.Math.Between(this.minPeriod, this.maxPeriod)
    }

    for (const f of this.fish) {
      if (f?.active) f.applyForce(this.strength * this.dirX * dt)
    }
  }

  startShake(ax: number, ay: number, forceMult: number) {
    this.isShaking = true
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
    // Direction follows shake axis
    this.dirX = ax < 0 ? -1 : 1
    this.elapsedTime = 0
  }

  continueShake(ax: number, ay: number, forceMult: number) {
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
    this.dirX = ax < 0 ? -1 : 1
    this.elapsedTime = 0
  }

  endShake() {
    this.isShaking = false
    this.magnitudeMult = 1
    this.shakeForceMultiplier = 1
  }
}
