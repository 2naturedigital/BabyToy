import Phaser from 'phaser'
import { SPRITES } from '../constants/assets'
import { useSettingsStore } from '../../store/settings'

const FONT = "'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif"

export class MenuScene extends Phaser.Scene {
  constructor() {
    super({ key: 'MenuScene' })
  }

  create() {
    const s = useSettingsStore().settings
    this.scale.setGameSize(s.landscapeMode ? 960 : 540, s.landscapeMode ? 540 : 960)
    const { width: W, height: H } = this.scale
    const landscape = W > H

    // Background
    const bg = this.add.image(W / 2, H / 2, SPRITES.BG)
    bg.setScale(Math.max(W / bg.width, H / bg.height))

    // Title — smaller in landscape so it leaves room for buttons
    const titleFontSize = landscape ? 72 : 96
    this.add.text(W / 2, H * (landscape ? 0.22 : 0.26), 'Rattler', {
      fontSize: `${titleFontSize}px`,
      color: '#ffffff',
      fontFamily: FONT,
      stroke: '#0066bb',
      strokeThickness: 8,
      shadow: { offsetX: 3, offsetY: 5, color: '#003366', blur: 6, fill: true },
    }).setOrigin(0.5)

    if (landscape) {
      // Side-by-side layout to avoid overlap
      const btnY = H * 0.56
      this.makeBtn(W * 0.30, btnY, 'Play', '#ffffff', 0x0099ee, 0x007acc, 60,
        () => {
          this.cameras.main.fadeOut(250, 0, 0, 0)
          this.cameras.main.once(Phaser.Cameras.Scene2D.Events.FADE_OUT_COMPLETE, () => {
            this.scene.start('FishTankScene')
          })
        })
      this.makeBtn(W * 0.70, btnY, 'Settings', '#cce6ff', 0x1a5580, 0x236b9e, 48,
        () => window.dispatchEvent(new Event('rattler:open-options')))
    } else {
      // Stacked layout for portrait
      this.makeBtn(W / 2, H * 0.46, 'Play', '#ffffff', 0x0099ee, 0x007acc, 72,
        () => {
          this.cameras.main.fadeOut(250, 0, 0, 0)
          this.cameras.main.once(Phaser.Cameras.Scene2D.Events.FADE_OUT_COMPLETE, () => {
            this.scene.start('FishTankScene')
          })
        })
      this.makeBtn(W / 2, H * 0.62, 'Settings', '#cce6ff', 0x1a5580, 0x236b9e, 52,
        () => window.dispatchEvent(new Event('rattler:open-options')))
    }

    // About link
    const about = this.add.text(W / 2, H * 0.88, 'About', {
      fontSize: '34px',
      color: '#4a7a9b',
      fontFamily: FONT,
    }).setOrigin(0.5).setInteractive({ useHandCursor: true })
    about.on('pointerup', () => window.dispatchEvent(new Event('rattler:open-about')))
    about.on('pointerover', () => about.setStyle({ color: '#7ab8d4' }))
    about.on('pointerout',  () => about.setStyle({ color: '#4a7a9b' }))

    this.add.text(W / 2, H * 0.94, 'v0.1-web', {
      fontSize: '24px',
      color: '#2a4a5b',
      fontFamily: FONT,
    }).setOrigin(0.5)

    this.cameras.main.fadeIn(350, 0, 0, 0)

    if (useSettingsStore().settings.firstRun) {
      this.time.delayedCall(400, () => {
        window.dispatchEvent(new Event('rattler:open-about'))
      })
    }
  }

  // Rounded-rectangle button: Graphics bg + Text label + invisible Zone for input
  private makeBtn(
    x: number, y: number,
    label: string, textColor: string,
    bgNorm: number, bgDown: number,
    fontSize: number,
    onUp: () => void,
  ) {
    const padX = 42, padY = 22, radius = 24

    // Measure text dimensions before drawing
    const tmp = this.add.text(0, -9999, label, {
      fontSize: `${fontSize}px`,
      fontFamily: FONT,
    })
    const bw = tmp.width  + padX * 2
    const bh = tmp.height + padY * 2
    tmp.destroy()

    const bg = this.add.graphics()
    const draw = (col: number) => {
      bg.clear()
      bg.fillStyle(col, 1)
      bg.fillRoundedRect(x - bw / 2, y - bh / 2, bw, bh, radius)
    }
    draw(bgNorm)

    this.add.text(x, y, label, {
      fontSize: `${fontSize}px`,
      color: textColor,
      fontFamily: FONT,
    }).setOrigin(0.5).setDepth(1)

    const zone = this.add.zone(x, y, bw, bh)
      .setInteractive({ useHandCursor: true })
      .setDepth(2)

    zone.on('pointerdown', () => draw(bgDown))
    zone.on('pointerout',  () => draw(bgNorm))
    zone.on('pointerup',   () => { draw(bgNorm); onUp() })
  }
}
