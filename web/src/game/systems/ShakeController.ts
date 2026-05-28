import type { FishController } from '../fish/FishController'
import type { WaterCurrent } from './WaterCurrent'
import type { TankCurrent } from './TankCurrent'
import type { BubbleSpawner } from '../objects/BubbleSpawner'
import { useSettingsStore } from '../../store/settings'

// Mirrors ShakeController.cs — receives shake events and broadcasts to all systems.
export class ShakeController {
  private fish: FishController[] = []
  private waterCurrents: WaterCurrent[] = []
  private bubbleSpawner?: BubbleSpawner
  private tankCurrent?: TankCurrent

  private isShaking = false
  private elapsedTime = 0
  private readonly shakeResetTimer = 0.3   // seconds of silence before EndShake

  register(f: FishController) { this.fish.push(f) }
  registerCurrent(c: WaterCurrent) { this.waterCurrents.push(c) }
  setBubbleSpawner(b: BubbleSpawner) { this.bubbleSpawner = b }
  setTankCurrent(t: TankCurrent) { this.tankCurrent = t }

  /** Called by Accelerometer when threshold is exceeded. */
  shake(ax: number, ay: number) {
    const forceMult = useSettingsStore().settings.shakePower * 2

    if (!this.isShaking) {
      this.isShaking = true
      for (const f of this.fish) f.startShake(ax, ay, forceMult)
      for (const c of this.waterCurrents) c.startShake(ax, ay, forceMult)
      this.bubbleSpawner?.startShake(ax, ay, forceMult)
      this.tankCurrent?.startShake(ax, ay, forceMult)
    } else {
      for (const f of this.fish) f.continueShake(ax, ay, forceMult)
      for (const c of this.waterCurrents) c.continueShake(ax, ay, forceMult)
      this.bubbleSpawner?.continueShake(ax, ay, forceMult)
      this.tankCurrent?.continueShake(ax, ay, forceMult)
    }

    this.elapsedTime = 0
  }

  update(delta: number) {
    if (!this.isShaking) return

    this.elapsedTime += delta / 1000
    if (this.elapsedTime > this.shakeResetTimer) {
      this.isShaking = false
      this.elapsedTime = 0
      for (const f of this.fish) f.endShake()
      for (const c of this.waterCurrents) c.endShake()
      this.bubbleSpawner?.endShake()
      this.tankCurrent?.endShake()
    }
  }
}
