import Phaser from 'phaser'
import { ANIM, AUDIO } from '../constants/assets'
import type { SoundController } from '../systems/SoundController'

const POP_KEYS = ['pop_0','pop_1','pop_2','pop_3','pop_4','pop_5','pop_6','pop_7']
const CREATE_KEYS = [AUDIO.BUBBLE_CREATE_1, AUDIO.BUBBLE_CREATE_2]
const POP_ANIMS = [ANIM.BUBBLE_POP, ANIM.BUBBLE_POP_TWO]
const LIFETIME_MIN = 2   // seconds
const LIFETIME_MAX = 8

export class Bubble extends Phaser.Physics.Arcade.Sprite {
  private lifetime: number
  private popped = false
  private snd: SoundController
  readonly physBody: Phaser.Physics.Arcade.Body

  constructor(scene: Phaser.Scene, x: number, y: number, snd: SoundController, scale: number, floatSpeed: number) {
    // Idle frame is bubble_uw_1 (un-popped bubble)
    super(scene, x, y, 'bubble_uw_1')
    scene.add.existing(this)
    scene.physics.add.existing(this)

    this.snd = snd
    this.lifetime = Phaser.Math.FloatBetween(LIFETIME_MIN, LIFETIME_MAX)
    this.physBody = this.body as Phaser.Physics.Arcade.Body

    // No world gravity — float upward via per-body gravity override
    this.physBody.setAllowGravity(false)
    this.physBody.setVelocityY(-floatSpeed)

    this.setScale(scale)
    this.setDepth(2)  // above water layer (1), below fish (3)
    this.setInteractive()

    // Randomize creation sound pitch/volume, alternating between two sounds
    const createKey = Phaser.Utils.Array.GetRandom(CREATE_KEYS) as string
    this.snd.play(createKey,
      Phaser.Math.FloatBetween(0.65, 0.95),
      Phaser.Math.FloatBetween(0.85, 1.2)
    )

    // Tap to pop
    this.on('pointerdown', () => this.pop())
  }

  bubbleUpdate(delta: number) {
    if (this.popped) return

    // Auto-destroy once above top of screen
    if (this.y < -this.displayHeight) {
      this.destroy()
      return
    }

    this.lifetime -= delta / 1000
    if (this.lifetime <= 0) {
      this.pop()
    }
  }

  pop() {
    if (this.popped) return
    this.popped = true
    this.disableInteractive()
    this.physBody.setVelocity(0, 0)

    this.snd.playRandom(POP_KEYS, Phaser.Math.FloatBetween(0.7, 1.0))

    const popAnim = Phaser.Utils.Array.GetRandom(POP_ANIMS) as string
    this.play(popAnim)
    this.once(Phaser.Animations.Events.ANIMATION_COMPLETE, () => this.destroy())
  }
}
