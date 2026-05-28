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
      width: 540,    // half of 1080 — cuts render work to ¼, same visual output
      height: 960,
    },
    render: {
      antialias: false,   // not needed for cartoon sprites, saves GPU time
      roundPixels: true,  // prevents sub-pixel shimmer on moving sprites
    },
    physics: {
      default: 'arcade',
      arcade: {
        gravity: { x: 0, y: 0 },
        debug: false,
      },
    },
    scene: [BootScene, MenuScene, FishTankScene],
    input: {
      activePointers: 4,
    },
  })
}
