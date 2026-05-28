import Phaser from 'phaser'
import type { Bubble } from '../objects/Bubble'

// Mirrors WaterCurrent.cs — a horizontal slice of the tank that nudges bubbles left or right.
export class WaterCurrent {
  // Rectangular zone in game-space pixels
  readonly left: number
  readonly right: number
  readonly top: number
  readonly bottom: number

  private direction = 1   // +1 = right, -1 = left
  private strength: number
  private magnitudeMult = 1
  private shakeForceMultiplier = 1
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

    // Alternate direction on a random timer
    this.elapsedTime += dt
    if (this.elapsedTime > this.movementPeriod) {
      this.elapsedTime = 0
      this.direction *= -1
      this.movementPeriod = Phaser.Math.Between(this.minPeriod, this.maxPeriod)
    }

    const force = this.strength * this.magnitudeMult * this.shakeForceMultiplier * this.direction

    for (const b of bubbles) {
      if (!b.active || !this.contains(b.x, b.y)) continue
      b.physBody.velocity.x += force * dt
    }
  }

  private contains(x: number, y: number) {
    return x >= this.left && x <= this.right && y >= this.top && y <= this.bottom
  }

  startShake(ax: number, ay: number, forceMult: number) {
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
  }

  continueShake(ax: number, ay: number, forceMult: number) {
    this.magnitudeMult = ax * ax + ay * ay
    this.shakeForceMultiplier = forceMult
  }

  endShake() {
    this.magnitudeMult = 1
    this.shakeForceMultiplier = 1
  }
}
