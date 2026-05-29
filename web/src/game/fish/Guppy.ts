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
        // Hold the shocked expression then animate back to normal
        this.scene.time.delayedCall(TAP_HOLD_MS, () => {
          if (!this.active) return
          this.recover()
        })
      })
    })

    this.play(ANIM.GUPPY_SWIM)
  }

  override startShake(ax: number, ay: number, forceMult: number) {
    super.startShake(ax, ay, forceMult)
    if (!this.reacting) {
      this.reacting = true
      this.play(ANIM.GUPPY_REACT)
    }
  }

  override endShake() {
    super.endShake()
    this.scene.time.delayedCall(SHAKE_HOLD_MS, () => {
      if (!this.active) return
      if (!this.isShaking) this.recover()
      else this.reacting = false
    })
  }

  // Play the expression-easing animation then return to swim
  private recover() {
    if (this.isShaking) { this.reacting = false; return }
    this.play(ANIM.GUPPY_RECOVER)
    this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
      if (!this.active) return
      this.reacting = false
      if (!this.isShaking) this.play(ANIM.GUPPY_SWIM)
    })
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
  }
}
