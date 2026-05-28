import Phaser from 'phaser'
import { SPRITES, AUDIO } from '../constants/assets'

export class BootScene extends Phaser.Scene {
  constructor() {
    super({ key: 'BootScene' })
  }

  preload() {
    this.createLoadingBar()
    this.loadSprites()
    this.loadAudio()
  }

  create() {
    this.createAnimations()
    this.scene.start('MenuScene')
  }

  private createLoadingBar() {
    const { width, height } = this.scale
    const cx = width / 2
    const cy = height / 2

    this.add.text(cx, cy - 80, 'Rattler', {
      fontSize: '80px',
      color: '#4fc3f7',
      fontFamily: 'Arial Rounded MT Bold, Arial, sans-serif',
    }).setOrigin(0.5)

    const barBorder = this.add.graphics()
    barBorder.lineStyle(4, 0x4fc3f7)
    barBorder.strokeRect(cx - 250, cy - 25, 500, 50)

    const barFill = this.add.graphics()

    this.load.on('progress', (value: number) => {
      barFill.clear()
      barFill.fillStyle(0x4fc3f7)
      barFill.fillRect(cx - 246, cy - 21, 492 * value, 42)
    })
  }

  private loadSprites() {
    // Backgrounds
    this.load.image(SPRITES.BG, '/assets/sprites/background/background_v2.png')
    this.load.image(SPRITES.BG_BLUR, '/assets/sprites/background/background_blurred_v3.png')
    this.load.image(SPRITES.WATER_LAYER, '/assets/sprites/background/waterlayer.png')
    this.load.image(SPRITES.HANDS_RIGHT, '/assets/sprites/background/fingers_cropped_v2.png')
    this.load.image(SPRITES.HANDS_LEFT, '/assets/sprites/background/fingers_cropped_v2_Left.png')

    // Bubbles — frame 1 is idle, frames 2-8 are the pop animation
    for (let i = 1; i <= 8; i++) {
      const pad = i.toString().padStart(2, '0')
      this.load.image(`bubble_uw_${i}`, `/assets/sprites/bubble_pop_underwater/bubble_pop_under_water_${pad}.png`)
    }
    for (let i = 1; i <= 7; i++) {
      const pad = i.toString().padStart(2, '0')
      this.load.image(`bubble_two_${i}`, `/assets/sprites/bubble_pop_two/buble_pop_two_${pad}.png`)
    }

    // Guppy
    this.load.image(SPRITES.GUPPY, '/assets/sprites/fish/guppy.png')
    for (let i = 1; i <= 4; i++) this.load.image(`guppy_swim_${i}`, `/assets/sprites/fish/guppy_swim${i}.png`)
    for (let i = 1; i <= 5; i++) this.load.image(`guppy_oshit_${i}`, `/assets/sprites/fish/guppy_oshit${i}.png`)

    // Starfish — 7 rotation frames, used as wobble animation
    for (let i = 1; i <= 7; i++) this.load.image(`starfish_${i}`, `/assets/sprites/starfish/starfish${i}_v2.png`)

    // BlowFish swim frames
    for (let i = 1; i <= 4; i++) this.load.image(`blowfish_swim_${i}`, `/assets/sprites/blowfish/blowfish_swim${i}.png`)
    // BlowFish inflated/reaction frames
    const inflated = ['1b','1e','1f','2b','2c','2d','2e','2f','3b','3c','3d','3e','3f','4','4e','4f']
    for (const f of inflated) this.load.image(`blowfish_${f}`, `/assets/sprites/blowfish/blowfish${f}.png`)

    // UI
    this.load.image(SPRITES.BTN, '/assets/sprites/parent_button/button.png')
    this.load.image(SPRITES.BTN_FILLED, '/assets/sprites/parent_button/button_filled.png')
  }

  private loadAudio() {
    // BlowFish
    for (let i = 1; i <= 6; i++) this.load.audio(`blowfish_swim_sfx_${i}`, `/assets/sfx/blowfish/swim${i}.wav`)
    this.load.audio(AUDIO.BF_INFLATE, '/assets/sfx/blowfish/blowfish_inflate_quiet.wav')
    this.load.audio(AUDIO.BF_DEFLATE, '/assets/sfx/blowfish/blowfish_deflate_quiet.wav')

    // Bubble pops
    for (let i = 0; i <= 7; i++) this.load.audio(`pop_${i}`, `/assets/sfx/bubbles_pop/pop${i}.wav`)

    // Bubble creation
    this.load.audio(AUDIO.BUBBLE_CREATE_1, '/assets/sfx/bubbles/bubble1.mp3')
    this.load.audio(AUDIO.BUBBLE_CREATE_2, '/assets/sfx/bubbles/bubbles.mp3')

    // Shake / water
    this.load.audio(AUDIO.SHAKE_LIGHT, '/assets/sfx/watershake/shake1_light.wav')
    this.load.audio(AUDIO.SHAKE_MEDIUM, '/assets/sfx/watershake/shake2_medium.wav')
    this.load.audio(AUDIO.SHAKE_MEDIUM_2, '/assets/sfx/watershake/shake2_second_half_medium.wav')
    this.load.audio(AUDIO.SHAKE_QUICK, '/assets/sfx/watershake/shake3_quick.wav')
  }

  private createAnimations() {
    const mgr = this.anims

    // Guppy swim loop
    mgr.create({
      key: 'guppy_swim',
      frames: [1,2,3,4].map(i => ({ key: `guppy_swim_${i}` })),
      frameRate: 8,
      repeat: -1,
    })

    // Guppy react (oshit), then play back-to-normal manually on complete
    mgr.create({
      key: 'guppy_react',
      frames: [1,2,3,4,5].map(i => ({ key: `guppy_oshit_${i}` })),
      frameRate: 10,
      repeat: 0,
    })

    // Starfish wobble — cycles through rotation frames
    mgr.create({
      key: 'starfish_wobble',
      frames: [1,2,3,4,5,6,7,6,5,4,3,2].map(i => ({ key: `starfish_${i}` })),
      frameRate: 12,
      repeat: -1,
    })

    // BlowFish swim loop
    mgr.create({
      key: 'blowfish_swim',
      frames: [1,2,3,4].map(i => ({ key: `blowfish_swim_${i}` })),
      frameRate: 8,
      repeat: -1,
    })

    // BlowFish inflate sequence (b=normal, e=mid, f=full)
    mgr.create({
      key: 'blowfish_inflate',
      frames: ['1b','1e','1f','2b','2c','2d','2e','2f','3b','3c','3d','3e','3f','4','4e','4f']
        .map(f => ({ key: `blowfish_${f}` })),
      frameRate: 12,
      repeat: 0,
    })

    // Bubble pop — two variants for visual variety
    mgr.create({
      key: 'bubble_pop',
      frames: [2,3,4,5,6,7,8].map(i => ({ key: `bubble_uw_${i}` })),
      frameRate: 16,
      repeat: 0,
    })
    mgr.create({
      key: 'bubble_pop_two',
      frames: [1,2,3,4,5,6,7].map(i => ({ key: `bubble_two_${i}` })),
      frameRate: 16,
      repeat: 0,
    })
  }
}
