import Phaser from 'phaser'
import type { Bubble } from '../objects/Bubble'

const MAX_BUBBLE_VX = 150  // px/s — prevents bubbles flying off screen

// Mirrors WaterCurrent.cs — a horizontal slice of the tank that nudges bubbles left or right.
export class WaterCurrent {
  readonly left: number
  readonly right: number
  readonly top: number
  readonly bottom: number

  private direction = 1
  private strength: number
  private magnitudeMult = 1
  private magnitudeWindRate = 0  // decay rate back to 1.0 after shake
  private elapsedTime = 0
  private movementPeriod: number
  private readonly minPeriod: number
  private readonly maxPeriod: number

  constructor(left: number, top: number, width: number, height: number, strength: number, minPeriod: number, maxPeriod: number) {
    this.left = left
    this.right = left + width
    this.top = top
    this.bottom = top + height
    this.strength = strength
    this.minPeriod = minPeriod
    this.maxPeriod = maxPeriod
    this.movementPeriod = Phaser.Math.Between(minPeriod, maxPeriod)
    this.direction = Math.random() < 0.5 ? -1 : 1
  }

  update(delta: number, bubbles: Bubble[]) {
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
      this.direction *= -1
      this.movementPeriod = Phaser.Math.Between(this.minPeriod, this.maxPeriod)
    }

    const force = this.strength * this.magnitudeMult * this.direction

    for (const b of bubbles) {
      if (!b.active || !this.contains(b.x, b.y)) continue
      const newVx = b.physBody.velocity.x + force * dt
      b.physBody.velocity.x = Phaser.Math.Clamp(newVx, -MAX_BUBBLE_VX, MAX_BUBBLE_VX)
    }
  }

  private contains(x: number, y: number) {
    return x >= this.left && x <= this.right && y >= this.top && y <= this.bottom
  }

  startShake(ax: number, ay: number, _forceMult: number) {
    // Cap current boost at 1.8× — don't use shakePower or squared magnitude here
    const mag = Math.sqrt(ax * ax + ay * ay)
    this.magnitudeMult = Math.min(1 + mag * 0.2, 1.8)
    this.magnitudeWindRate = 0
  }

  continueShake(ax: number, ay: number, _forceMult: number) {
    const mag = Math.sqrt(ax * ax + ay * ay)
    this.magnitudeMult = Math.min(1 + mag * 0.2, 1.8)
  }

  endShake() {
    // Let magnitudeMult decay gradually over 2 seconds
    this.magnitudeWindRate = (this.magnitudeMult - 1.0) / 2.0
  }
}
