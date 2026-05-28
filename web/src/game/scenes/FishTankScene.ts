import Phaser from 'phaser'
import { SPRITES } from '../constants/assets'

// Phase 2 will replace this stub with the full fish tank implementation.
export class FishTankScene extends Phaser.Scene {
  private longPressTimer?: Phaser.Time.TimerEvent
  private holdProgress?: Phaser.GameObjects.Graphics

  constructor() {
    super({ key: 'FishTankScene' })
  }

  create() {
    const { width, height } = this.scale

    const bg = this.add.image(width / 2, height / 2, SPRITES.BG)
    bg.setDisplaySize(width, height)

    this.add.text(width / 2, height / 2, '🐟  Fish Tank\ncoming in Phase 2', {
      fontSize: '52px',
      color: '#ffffff',
      align: 'center',
      fontFamily: 'Arial, sans-serif',
      backgroundColor: '#00000066',
      padding: { x: 30, y: 20 },
    }).setOrigin(0.5)

    this.setupParentButton()
  }

  // Long-press the parent button (3 s) to return to menu — mirrors LongClickButton.cs
  private setupParentButton() {
    const { width } = this.scale
    const x = width - 80
    const y = 80

    const btn = this.add.image(x, y, SPRITES.BTN)
      .setDisplaySize(100, 100)
      .setInteractive({ useHandCursor: true })
      .setAlpha(0.7)

    this.holdProgress = this.add.graphics()

    btn.on('pointerdown', () => {
      this.holdProgress!.clear()
      this.longPressTimer = this.time.addEvent({
        delay: 3000,
        callback: () => {
          this.holdProgress!.clear()
          this.scene.start('MenuScene')
        },
      })

      // Animate fill arc while held
      this.tweens.addCounter({
        from: 0,
        to: 1,
        duration: 3000,
        onUpdate: (tween) => {
          const v = tween.getValue()
          this.holdProgress!.clear()
          this.holdProgress!.fillStyle(0x4fc3f7, 0.6)
          this.holdProgress!.slice(x, y, 54, -Math.PI / 2, -Math.PI / 2 + v * Math.PI * 2, false)
          this.holdProgress!.fillPath()
        },
      })
    })

    btn.on('pointerup', () => this.cancelLongPress())
    btn.on('pointerout', () => this.cancelLongPress())
  }

  private cancelLongPress() {
    this.longPressTimer?.remove()
    this.holdProgress?.clear()
    this.tweens.killAll()
  }
}
