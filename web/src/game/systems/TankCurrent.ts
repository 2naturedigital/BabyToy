import Phaser from 'phaser'
import type { BlowFish } from '../fish/BlowFish'

// Mirrors TankCurrent.cs — applies a slow horizontal drift to BlowFish only.
export class TankCurrent {
  private fish: BlowFish[] = []
  private dirX = 1
  private strength: number
  private magnitudeMult = 1
  private magnitudeWindRate = 0  // decay rate back to 1.0 after shake
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

    // Gradually wind down shake boost
    if (this.magnitudeWindRate > 0) {
      this.magnitudeMult -= this.magnitudeWindRate * dt
      if (this.magnitudeMult <= 1.0) {
        this.magnitudeMult = 1.0
        this.magnitudeWindRate = 0
      }
    }

    this.elapsedTime += dt
    if (this.elapsedTime > this.movementPeriod) {
      this.elapsedTime = 0
      if (!this.isShaking) this.dirX *= -1
      this.strength = Phaser.Math.FloatBetween(this.minStr, this.maxStr)
      this.movementPeriod = Phaser.Math.Between(this.minPeriod, this.maxPeriod)
    }

    // magnitudeMult applied at force point so gradual wind-down takes effect immediately
    for (const f of this.fish) {
      if (f?.active) f.applyForce(this.strength * this.magnitudeMult * this.dirX * dt)
    }
  }

  startShake(ax: number, ay: number, _forceMult: number) {
    this.isShaking = true
    const mag = Math.sqrt(ax * ax + ay * ay)
    this.magnitudeMult = Math.min(1 + mag * 0.3, 2.0)
    this.magnitudeWindRate = 0
    this.dirX = ax < 0 ? -1 : 1
    this.elapsedTime = 0
  }

  continueShake(ax: number, ay: number, _forceMult: number) {
    const mag = Math.sqrt(ax * ax + ay * ay)
    this.magnitudeMult = Math.min(1 + mag * 0.3, 2.0)
    this.dirX = ax < 0 ? -1 : 1
    this.elapsedTime = 0
  }

  endShake() {
    this.isShaking = false
    // Wind down over 2 seconds
    this.magnitudeWindRate = (this.magnitudeMult - 1.0) / 2.0
  }
}
