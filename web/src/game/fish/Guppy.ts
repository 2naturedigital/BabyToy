import Phaser from 'phaser'
import { FishController } from './FishController'
import { ANIM } from '../constants/assets'

export class Guppy extends FishController {
  private reacting = false

  constructor(scene: Phaser.Scene, x: number, y: number) {
    super(scene, x, y, 'guppy_swim_1', 80, 220)
  }

  override init(snd: import('../systems/SoundController').SoundController, size: number) {
    super.init(snd, size)

    // Tap → react animation, then return to swim
    this.on('pointerdown', () => {
      if (this.reacting) return
      this.reacting = true
      this.play(ANIM.GUPPY_REACT)
      this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => {
        this.reacting = false
        this.play(ANIM.GUPPY_SWIM)
      })
    })

    this.play(ANIM.GUPPY_SWIM)
  }

  fishUpdate(delta: number) {
    this.moveFish(delta)
  }
}
