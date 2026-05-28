import Phaser from 'phaser'
import { useSettingsStore } from '../../store/settings'

export class SoundController {
  private scene: Phaser.Scene

  constructor(scene: Phaser.Scene) {
    this.scene = scene
  }

  play(key: string, volume = 1.0, rate = 1.0, stopOthers = false) {
    const vol = volume * useSettingsStore().settings.volume
    if (stopOthers) this.scene.sound.stopAll()
    this.scene.sound.play(key, { volume: vol, rate })
  }

  playRandom(keys: string[], volume = 1.0) {
    const key = Phaser.Utils.Array.GetRandom(keys) as string
    this.play(key, volume, Phaser.Math.FloatBetween(0.85, 1.15))
  }
}
