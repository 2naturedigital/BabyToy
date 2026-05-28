import { useSettingsStore } from '../../store/settings'

// Mirrors Accelerometer.cs — detects device shake via DeviceMotionEvent.
// Falls back to spacebar on desktop for testing.
export class Accelerometer {
  private readonly onShake: (ax: number, ay: number) => void
  private lastShakeTime = 0
  private readonly minInterval = 0.1  // seconds between shake triggers
  private motionGranted = false
  private readonly motionHandler: (e: DeviceMotionEvent) => void

  constructor(onShake: (ax: number, ay: number) => void) {
    this.onShake = onShake
    // Bind once so the same reference can be passed to removeEventListener
    this.motionHandler = (e: DeviceMotionEvent) => {
      const acc = e.accelerationIncludingGravity
      if (!acc) return
      // Normalise to Unity's scale (gravity = 1.0) by dividing by 9.81
      const ax = (acc.x ?? 0) / 9.81
      const ay = (acc.y ?? 0) / 9.81
      const az = (acc.z ?? 0) / 9.81
      const sqrMag = ax * ax + ay * ay + az * az

      const sensitivity = useSettingsStore().settings.shakeSensitivity
      // Default threshold 1.6 (sqrMagnitude), scaled by sensitivity (0.5–2.5)
      const threshold = 1.6 / sensitivity

      const now = performance.now() / 1000
      if (sqrMag >= threshold && now >= this.lastShakeTime + this.minInterval) {
        this.lastShakeTime = now
        this.onShake(ax, ay)
      }
    }
  }

  /** Must be called from a user-gesture context (scene create). */
  async start() {
    // iOS 13+ requires explicit permission
    if (
      typeof DeviceMotionEvent !== 'undefined' &&
      typeof (DeviceMotionEvent as unknown as { requestPermission?: () => Promise<string> }).requestPermission === 'function'
    ) {
      try {
        const perm = await (DeviceMotionEvent as unknown as { requestPermission: () => Promise<string> }).requestPermission()
        if (perm === 'granted') this.attachListener()
      } catch {
        // denied — motion stays disabled, spacebar still works
      }
    } else {
      this.attachListener()
    }
  }

  stop() {
    window.removeEventListener('devicemotion', this.motionHandler)
    this.motionGranted = false
  }

  private attachListener() {
    this.motionGranted = true
    window.addEventListener('devicemotion', this.motionHandler)
  }

  /** Desktop testing — call from scene update with Phaser cursor keys. */
  simulateShake() {
    const now = performance.now() / 1000
    if (now >= this.lastShakeTime + this.minInterval) {
      this.lastShakeTime = now
      this.onShake(1.2, 0.8)
    }
  }
}
