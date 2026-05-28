import Phaser from 'phaser'
import { SPRITES } from '../constants/assets'

export class MenuScene extends Phaser.Scene {
  constructor() {
    super({ key: 'MenuScene' })
  }

  create() {
    const { width, height } = this.scale

    // Background
    const bg = this.add.image(width / 2, height / 2, SPRITES.BG)
    bg.setDisplaySize(width, height)

    // Title
    this.add.text(width / 2, height * 0.28, 'Rattler', {
      fontSize: '140px',
      color: '#ffffff',
      fontFamily: 'Arial Rounded MT Bold, Arial, sans-serif',
      stroke: '#0077cc',
      strokeThickness: 10,
      shadow: { offsetX: 4, offsetY: 6, color: '#003366', blur: 8, fill: true },
    }).setOrigin(0.5)

    // Play button
    const playBtn = this.add.text(width / 2, height * 0.52, 'Play', {
      fontSize: '90px',
      color: '#ffffff',
      backgroundColor: '#0099ee',
      padding: { x: 80, y: 35 },
      fontFamily: 'Arial Rounded MT Bold, Arial, sans-serif',
    })
      .setOrigin(0.5)
      .setInteractive({ useHandCursor: true })

    playBtn.on('pointerdown', () => {
      playBtn.setStyle({ backgroundColor: '#007acc' })
    })
    playBtn.on('pointerup', () => {
      this.scene.start('FishTankScene')
    })
    playBtn.on('pointerover', () => playBtn.setStyle({ backgroundColor: '#00bbff' }))
    playBtn.on('pointerout', () => playBtn.setStyle({ backgroundColor: '#0099ee' }))

    // Settings button
    const settingsBtn = this.add.text(width / 2, height * 0.65, 'Settings', {
      fontSize: '60px',
      color: '#cce6ff',
      backgroundColor: '#1a5580',
      padding: { x: 50, y: 22 },
      fontFamily: 'Arial Rounded MT Bold, Arial, sans-serif',
    })
      .setOrigin(0.5)
      .setInteractive({ useHandCursor: true })

    settingsBtn.on('pointerup', () => {
      window.dispatchEvent(new Event('rattler:open-options'))
    })
    settingsBtn.on('pointerover', () => settingsBtn.setStyle({ backgroundColor: '#236b9e' }))
    settingsBtn.on('pointerout', () => settingsBtn.setStyle({ backgroundColor: '#1a5580' }))

    // Version tag
    this.add.text(width / 2, height * 0.92, 'v0.1 — web port', {
      fontSize: '32px',
      color: '#4a7a9b',
      fontFamily: 'Arial, sans-serif',
    }).setOrigin(0.5)
  }
}
