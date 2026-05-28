import Phaser from 'phaser'
import { BootScene } from './scenes/BootScene'
import { MenuScene } from './scenes/MenuScene'
import { FishTankScene } from './scenes/FishTankScene'

export function createGame(parent: HTMLElement): Phaser.Game {
  return new Phaser.Game({
    type: Phaser.AUTO,
    parent,
    backgroundColor: '#001a33',
    scale: {
      mode: Phaser.Scale.FIT,
      autoCenter: Phaser.Scale.CENTER_BOTH,
      width: 1080,
      height: 1920,
    },
    physics: {
      default: 'arcade',
      arcade: {
        gravity: { x: 0, y: 150 },
        debug: false,
      },
    },
    scene: [BootScene, MenuScene, FishTankScene],
    input: {
      activePointers: 4,
    },
  })
}
