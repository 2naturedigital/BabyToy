import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM } from '../constants/assets'

// How long (ms) guppy holds its shocked expression after the react anim completes
const TAP_HOLD_MS   = 1200
const SHAKE_HOLD_MS = 2000

export class Guppy extends FishController {
  private reacting = false

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'guppy_swim_1', 80, 220)
    this.baseScale = 0.36
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.play(ANIM.GUPPY_REACT)
      this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
        // Hold the shocked expression before returning to swim
        this.scene.time.delayedCall(TAP_HOLD_MS, () => {
          if (!this.active) return
          this.reacting = false
          if (!this.isShaking) this.play(ANIM.GUPPY_SWIM)
        })
      })
    })

    this.play(ANIM.GUPPY_SWIM)
  }

  // Guppy gets scared when the device is shaken too
  override startShake(ax: number, ay: number, forceMult: number) {
    super.startShake(ax, ay, forceMult)
    if (!this.reacting) {
      this.reacting = true
      this.play(ANIM.GUPPY_REACT)
      // Hold on last frame until endShake fires
    }
  }

  override endShake() {
    super.endShake()
    // Keep the shocked face for a beat after the shake stops
    this.scene.time.delayedCall(SHAKE_HOLD_MS, () => {
      if (!this.active) return
      this.reacting = false
      if (!this.isShaking) this.play(ANIM.GUPPY_SWIM)
    })
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
  }
}
