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

  private holdGraphics?: Phaser.GameObjects.Graphics
  private holdTween?: Phaser.Tweens.Tween
  private holdTimer?: Phaser.Time.TimerEvent
  private spaceKey?: Phaser.Input.Keyboard.Key

  // Arrow function so removeEventListener works correctly
  private readonly onSettingsApplied = () => this.scene.restart()
  private readonly onOpenOptions = () => window.dispatchEvent(new Event('rattler:open-options'))

  constructor() {
    super({ key: 'FishTankScene' })
  }

  create() {
    const { width: W, height: H } = this.scale
    const s = useSettingsStore().settings

    // ── Background ──────────────────────────────────────────────────────────
    const bgKey = s.blurBackground ? SPRITES.BG_BLUR : SPRITES.BG
    const bg = this.add.image(W / 2, H / 2, bgKey).setDepth(0)
    bg.setScale(Math.max(W / bg.width, H / bg.height))

    if (s.showWaterLayer) {
      const water = this.add.image(W / 2, H / 2, SPRITES.WATER_LAYER).setDepth(1)
      water.setScale(Math.max(W / water.width, H / water.height))
      water.setAlpha(0.3)
    }

    // ── Core systems ─────────────────────────────────────────────────────────
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

    this.blowfish = new BlowFish(this, W * 0.5, H * 0.35)
    this.blowfish.init(this.snd, s.blowfishSize)
    this.blowfish.setDepth(3)

    this.shakeCtrl.register(this.guppy)
    this.shakeCtrl.register(this.starfish)
    this.shakeCtrl.register(this.blowfish)

    // ── Bubbles (depth 2) ────────────────────────────────────────────────────
    this.bubbleSpawner = new BubbleSpawner(this, this.snd)
    this.shakeCtrl.setBubbleSpawner(this.bubbleSpawner)

    // ── Water currents — 4 horizontal slices ────────────────────────────────
    const sliceH = H / 4
    this.waterCurrents = [0, 1, 2, 3].map(i =>
      new WaterCurrent(0, i * sliceH, W, sliceH, 40, 2, 6)
    )
    for (const c of this.waterCurrents) this.shakeCtrl.registerCurrent(c)

    // ── Tank current (BlowFish horizontal drift) ─────────────────────────────
    this.tankCurrent = new TankCurrent(20, 80, 2, 8)
    this.tankCurrent.setFish(this.blowfish)
    this.shakeCtrl.setTankCurrent(this.tankCurrent)

    // ── Hands (depth 5) ──────────────────────────────────────────────────────
    if (s.showHands) {
      const rh = this.add.image(0, 0, SPRITES.HANDS_RIGHT).setDepth(5).setOrigin(1, 1)
      const lh = this.add.image(0, 0, SPRITES.HANDS_LEFT).setDepth(5).setOrigin(0, 1)
      const handScale = (W * 0.42) / rh.width  // was 0.55 — trimmed down
      rh.setScale(handScale).setPosition(W, H)
      lh.setScale(handScale).setPosition(0, H)
    }

    // ── Parent long-press button (top-right, depth 10) ───────────────────────
    this.setupParentButton(W, H)

    // ── HUD buttons: shake + rotate (top-left, depth 10) ─────────────────────
    this.setupHudButtons(W)

    // ── Keyboard: spacebar = shake, P = open settings ───────────────────────
    if (this.input.keyboard) {
      this.spaceKey = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.SPACE)
      this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.P).on('down', () => {
        window.dispatchEvent(new Event('rattler:open-options'))
      })
    }

    // ── Global event listeners ───────────────────────────────────────────────
    window.addEventListener('rattler:settings-applied', this.onSettingsApplied)

    // Clean up listeners when scene shuts down
    this.events.once(Phaser.Scenes.Events.SHUTDOWN, () => {
      window.removeEventListener('rattler:settings-applied', this.onSettingsApplied)
      this.accel.stop()
    })

    // Fade in + start motion sensor
    this.cameras.main.fadeIn(350, 0, 0, 0)
    this.accel.start()
  }

  update(_time: number, delta: number) {
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

  private setupParentButton(W: number, H: number) {
    const x = W - 36
    const y = 36
    this.holdGraphics = this.add.graphics().setDepth(10)

    const btn = this.add.image(x, y, SPRITES.BTN)
      .setDisplaySize(60, 60)
      .setAlpha(0.6)
      .setDepth(10)
      .setInteractive({ useHandCursor: true })

    btn.on('pointerdown', () => {
      this.holdTimer = this.time.addEvent({
        delay: 3000,
        callback: () => {
          this.holdGraphics?.clear()
          this.cameras.main.fadeOut(250, 0, 0, 0)
          this.cameras.main.once(Phaser.Cameras.Scene2D.Events.FADE_OUT_COMPLETE, () => {
            this.scene.start('MenuScene')
          })
        },
      })
      this.holdTween = this.tweens.addCounter({
        from: 0, to: 1, duration: 3000,
        onUpdate: (tw) => {
          const v = tw.getValue()
          this.holdGraphics?.clear()
          this.holdGraphics?.fillStyle(0x4fc3f7, 0.7)
          this.holdGraphics?.slice(x, y, 34, -Math.PI / 2, -Math.PI / 2 + v * Math.PI * 2, false)
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

  private setupHudButtons(W: number) {
    const makeBtn = (x: number, label: string) => {
      const img = this.add.image(x, 36, SPRITES.BTN)
        .setDisplaySize(52, 52).setAlpha(0.5).setDepth(10)
        .setInteractive({ useHandCursor: true })
      this.add.text(x, 36, label, {
        fontSize: '22px',
        color: '#ffffff',
        fontFamily: 'Arial, sans-serif',
      }).setOrigin(0.5).setDepth(11)
      return img
    }

    // Shake button — trigger an artificial shake event
    const shakeBtn = makeBtn(36, '〜')
    shakeBtn.on('pointerup', () => this.accel.simulateShake())
    shakeBtn.on('pointerover', () => shakeBtn.setAlpha(0.8))
    shakeBtn.on('pointerout',  () => shakeBtn.setAlpha(0.5))

    // Rotate button — request landscape/portrait lock via Screen Orientation API
    const rotBtn = makeBtn(96, '↺')
    rotBtn.on('pointerup', () => {
      if (!screen.orientation || typeof (screen.orientation as any).lock !== 'function') return
      const isPortrait = screen.orientation.type.startsWith('portrait')
      try {
        if (isPortrait) {
          void (screen.orientation as any).lock('landscape-primary')
        } else {
          screen.orientation.unlock()
        }
      } catch { /* not available on desktop */ }
    })
    rotBtn.on('pointerover', () => rotBtn.setAlpha(0.8))
    rotBtn.on('pointerout',  () => rotBtn.setAlpha(0.5))
  }
}
