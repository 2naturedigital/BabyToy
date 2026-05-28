import Phaser from 'phaser'
import { SPRITES } from '../constants/assets'
import { SoundController } from '../systems/SoundController'
import { ShakeController } from '../systems/ShakeController'
import { Accelerometer } from '../systems/Accelerometer'
import { WaterCurrent } from '../systems/WaterCurrent'
import { TankCurrent } from '../systems/TankCurrent'
import { BubbleSpawner } from '../objects/BubbleSpawner'
import { Guppy } from '../fish/Guppy'
import { Starfish } from '../fish/Starfish'
import { BlowFish } from '../fish/BlowFish'
import { useSettingsStore } from '../../store/settings'

export class FishTankScene extends Phaser.Scene {
  private snd!: SoundController
  private shakeCtrl!: ShakeController
  private accel!: Accelerometer
  private bubbleSpawner!: BubbleSpawner
  private waterCurrents!: WaterCurrent[]
  private tankCurrent!: TankCurrent
  private guppy!: Guppy
  private starfish!: Starfish
  private blowfish!: BlowFish

  // Parent long-press button state
  private holdGraphics?: Phaser.GameObjects.Graphics
  private holdTween?: Phaser.Tweens.Tween
  private holdTimer?: Phaser.Time.TimerEvent

  private spaceKey?: Phaser.Input.Keyboard.Key

  constructor() {
    super({ key: 'FishTankScene' })
  }

  create() {
    const { width: W, height: H } = this.scale
    const s = useSettingsStore().settings

    // ── Background — cover-fit: fills canvas without stretching ─────────────
    const bgKey = s.blurBackground ? SPRITES.BG_BLUR : SPRITES.BG
    const bg = this.add.image(W / 2, H / 2, bgKey).setDepth(0)
    bg.setScale(Math.max(W / bg.width, H / bg.height))

    if (s.showWaterLayer) {
      const water = this.add.image(W / 2, H / 2, SPRITES.WATER_LAYER).setDepth(1)
      water.setScale(Math.max(W / water.width, H / water.height))
      water.setAlpha(0.3)
    }

    // ── Core systems ────────────────────────────────────────────────────────
    this.snd = new SoundController(this)
    this.shakeCtrl = new ShakeController()
    this.accel = new Accelerometer((ax, ay) => this.shakeCtrl.shake(ax, ay))

    // ── Fish (depth 3) ───────────────────────────────────────────────────────
    this.guppy = new Guppy(this, W * 0.3, H * 0.5)
    this.guppy.init(this.snd, s.guppySize)
    this.guppy.setDepth(3)

    this.starfish = new Starfish(this, W * 0.7, H * 0.4)
    this.starfish.init(this.snd, s.starfishSize)
    this.starfish.setDepth(3)

    this.blowfish = new BlowFish(this, W * 0.5, H * 0.6)
    this.blowfish.init(this.snd, s.blowfishSize)
    this.blowfish.setDepth(3)

    this.shakeCtrl.register(this.guppy)
    this.shakeCtrl.register(this.starfish)
    this.shakeCtrl.register(this.blowfish)

    // ── Bubbles (depth 2) ────────────────────────────────────────────────────
    this.bubbleSpawner = new BubbleSpawner(this, this.snd)
    this.shakeCtrl.setBubbleSpawner(this.bubbleSpawner)

    // ── Water currents — 4 horizontal zones stacked vertically (depth 0) ────
    // Mirrors CurrentInitializer.cs — slices the tank into quarters
    const sliceH = H / 4
    this.waterCurrents = [0, 1, 2, 3].map(i =>
      new WaterCurrent(0, i * sliceH, W, sliceH, 40, 2, 6)
    )
    for (const c of this.waterCurrents) this.shakeCtrl.registerCurrent(c)

    // ── Tank current (affects BlowFish horizontal drift) ─────────────────────
    this.tankCurrent = new TankCurrent(20, 80, 2, 8)
    this.tankCurrent.setFish(this.blowfish)
    this.shakeCtrl.setTankCurrent(this.tankCurrent)

    // ── Hand overlays — anchored to bottom corners like the Unity original ────
    if (s.showHands) {
      // Each hand fills ~55% of screen width, anchored at bottom-right / bottom-left
      const rh = this.add.image(0, 0, SPRITES.HANDS_RIGHT).setDepth(5).setOrigin(1, 1)
      const lh = this.add.image(0, 0, SPRITES.HANDS_LEFT).setDepth(5).setOrigin(0, 1)
      const handScale = (W * 0.55) / rh.width
      rh.setScale(handScale).setPosition(W, H)
      lh.setScale(handScale).setPosition(0, H)
    }

    // ── Parent long-press button (top-right, depth 10) ─────────────────────
    this.setupParentButton(W, H)

    // ── Keyboard shake (desktop testing) ─────────────────────────────────────
    this.spaceKey = this.input.keyboard?.addKey(Phaser.Input.Keyboard.KeyCodes.SPACE)

    // Start motion sensor (async — non-blocking)
    this.accel.start()
  }

  update(_time: number, delta: number) {
    // Spacebar simulates a device shake for desktop testing
    if (this.spaceKey && Phaser.Input.Keyboard.JustDown(this.spaceKey)) {
      this.accel.simulateShake()
    }

    this.guppy.fishUpdate(delta)
    this.starfish.fishUpdate(delta)
    this.blowfish.fishUpdate(delta)
    this.bubbleSpawner.update(delta)

    const bubbles = this.bubbleSpawner.bubbles
    for (const c of this.waterCurrents) c.update(delta, bubbles)
    this.tankCurrent.update(delta)
    this.shakeCtrl.update(delta)
  }

  // ── Parent long-press button — mirrors LongClickButton.cs (3 s hold) ──────
  private setupParentButton(W: number, H: number) {
    const x = W - 60
    const y = 60
    this.holdGraphics = this.add.graphics().setDepth(10)

    const btn = this.add.image(x, y, SPRITES.BTN)
      .setDisplaySize(90, 90)
      .setAlpha(0.65)
      .setDepth(10)
      .setInteractive({ useHandCursor: true })

    btn.on('pointerdown', () => {
      this.holdTimer = this.time.addEvent({
        delay: 3000,
        callback: () => {
          this.holdGraphics?.clear()
          this.scene.start('MenuScene')
        },
      })
      this.holdTween = this.tweens.addCounter({
        from: 0, to: 1, duration: 3000,
        onUpdate: (tw) => {
          const v = tw.getValue()
          this.holdGraphics?.clear()
          this.holdGraphics?.fillStyle(0x4fc3f7, 0.6)
          this.holdGraphics?.slice(x, y, 50, -Math.PI / 2, -Math.PI / 2 + v * Math.PI * 2, false)
          this.holdGraphics?.fillPath()
        },
      })
    })

    const cancelHold = () => {
      this.holdTimer?.remove()
      this.holdTween?.stop()
      this.holdGraphics?.clear()
    }
    btn.on('pointerup', cancelHold)
    btn.on('pointerout', cancelHold)
  }
}
