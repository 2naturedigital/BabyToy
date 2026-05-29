import Phaser from 'phaser'
import { SPRITES } from '../constants/assets'
import { useSettingsStore } from '../../store/settings'

export class MenuScene extends Phaser.Scene {
  constructor() {
    super({ key: 'MenuScene' })
  }

  create() {
    const s = useSettingsStore().settings
    this.scale.setGameSize(s.landscapeMode ? 960 : 540, s.landscapeMode ? 540 : 960)
    const { width: W, height: H } = this.scale

    // Background — cover-fit (no stretch)
    const bg = this.add.image(W / 2, H / 2, SPRITES.BG)
    bg.setScale(Math.max(W / bg.width, H / bg.height))

    // Title
    this.add.text(W / 2, H * 0.26, 'Rattler', {
      fontSize: '96px',
      color: '#ffffff',
      fontFamily: "'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif",
      stroke: '#0066bb',
      strokeThickness: 8,
      shadow: { offsetX: 3, offsetY: 5, color: '#003366', blur: 6, fill: true },
    }).setOrigin(0.5)

    // Play button
    const playBtn = this.add.text(W / 2, H * 0.46, '  Play  ', {
      fontSize: '72px',
      color: '#ffffff',
      backgroundColor: '#0099ee',
      padding: { x: 40, y: 20 },
      fontFamily: "'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif",
    }).setOrigin(0.5).setInteractive({ useHandCursor: true })

    playBtn.on('pointerdown', () => playBtn.setStyle({ backgroundColor: '#007acc' }))
    playBtn.on('pointerout',  () => playBtn.setStyle({ backgroundColor: '#0099ee' }))
    playBtn.on('pointerup',   () => {
      this.cameras.main.fadeOut(250, 0, 0, 0)
      this.cameras.main.once(Phaser.Cameras.Scene2D.Events.FADE_OUT_COMPLETE, () => {
        this.scene.start('FishTankScene')
      })
    })

    // Settings button
    const settingsBtn = this.add.text(W / 2, H * 0.60, 'Settings', {
      fontSize: '52px',
      color: '#cce6ff',
      backgroundColor: '#1a5580',
      padding: { x: 36, y: 16 },
      fontFamily: "'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif",
    }).setOrigin(0.5).setInteractive({ useHandCursor: true })

    settingsBtn.on('pointerdown', () => settingsBtn.setStyle({ backgroundColor: '#236b9e' }))
    settingsBtn.on('pointerout',  () => settingsBtn.setStyle({ backgroundColor: '#1a5580' }))
    settingsBtn.on('pointerup',   () => {
      window.dispatchEvent(new Event('rattler:open-options'))
    })

    // About link (small, bottom)
    const about = this.add.text(W / 2, H * 0.88, 'About', {
      fontSize: '34px',
      color: '#4a7a9b',
      fontFamily: 'Arial, sans-serif',
    }).setOrigin(0.5).setInteractive({ useHandCursor: true })
    about.on('pointerup', () => window.dispatchEvent(new Event('rattler:open-about')))

    this.add.text(W / 2, H * 0.94, 'v0.1-web', {
      fontSize: '24px',
      color: '#2a4a5b',
      fontFamily: 'Arial, sans-serif',
    }).setOrigin(0.5)

    // Fade in
    this.cameras.main.fadeIn(350, 0, 0, 0)

    // First-run → show About modal
    if (useSettingsStore().settings.firstRun) {
      // slight delay so the scene is fully visible first
      this.time.delayedCall(400, () => {
        window.dispatchEvent(new Event('rattler:open-about'))
      })
    }
  }
}
