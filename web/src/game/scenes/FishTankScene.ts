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

// Index 0 = original colour; 1-3 are tinted variants (max 3 fish per species)
const GUPPY_TINTS    = [0xffffff, 0xff8888, 0x88ffff, 0xffff88]
const STARFISH_TINTS = [0xffffff, 0xffaa44, 0x44ddff, 0x88ff44]
const BLOWFISH_TINTS = [0xffffff, 0x88ffcc, 0xffcc88, 0xccaaff]

export class FishTankScene extends Phaser.Scene {
  private snd!: SoundController
  private shakeCtrl!: ShakeController
  private accel!: Accelerometer
  private bubbleSpawner!: BubbleSpawner
  private waterCurrents!: WaterCurrent[]
  private tankCurrent!: TankCurrent

  private guppies:    Guppy[]    = []
  private starfishes: Starfish[] = []
  private blowfishes: BlowFish[] = []

  private holdGraphics?: Phaser.GameObjects.Graphics
  private holdTween?: Phaser.Tweens.Tween
  private holdTimer?: Phaser.Time.TimerEvent
  private spaceKey?: Phaser.Input.Keyboard.Key

  private readonly onSettingsApplied = () => this.scene.restart()
  private readonly onOverlayOpen  = () => { this.input.enabled = false }
  private readonly onOverlayClose = () => { this.input.enabled = true }

  constructor() {
    super({ key: 'FishTankScene' })
  }

  create() {
    const s = useSettingsStore().settings

    // ── Canvas orientation ────────────────────────────────────────────────────
    this.scale.setGameSize(s.landscapeMode ? 960 : 540, s.landscapeMode ? 540 : 960)
    const { width: W, height: H } = this.scale

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

    // ── Fish ─────────────────────────────────────────────────────────────────
    // Spread fish across the tank, avoiding overlap by dividing the space evenly
    this.guppies    = []
    this.starfishes = []
    this.blowfishes = []

    // Each fish gets a randomised size within ±fishSizeVariation of the master
    const randSize = () => {
      const v = s.fishSizeVariation
      return s.fishSize * Phaser.Math.FloatBetween(1 - v, 1 + v)
    }

    for (let i = 0; i < s.guppyCount; i++) {
      const fish = new Guppy(this,
        Phaser.Math.FloatBetween(W * 0.1, W * 0.9),
        Phaser.Math.FloatBetween(H * 0.3, H * 0.7),
      )
      fish.init(this.snd, randSize())
      fish.setDepth(4)
      fish.setTint(GUPPY_TINTS[i % GUPPY_TINTS.length])
      this.guppies.push(fish)
      this.shakeCtrl.register(fish)
    }

    for (let i = 0; i < s.starfishCount; i++) {
      const fish = new Starfish(this,
        Phaser.Math.FloatBetween(W * 0.1, W * 0.9),
        Phaser.Math.FloatBetween(H * 0.2, H * 0.8),
      )
      fish.init(this.snd, randSize())
      fish.setDepth(5)
      fish.setTint(STARFISH_TINTS[i % STARFISH_TINTS.length])
      this.starfishes.push(fish)
      this.shakeCtrl.register(fish)
    }

    this.tankCurrent = new TankCurrent(20, 80, 2, 8)

    for (let i = 0; i < s.blowfishCount; i++) {
      const fish = new BlowFish(this,
        Phaser.Math.FloatBetween(W * 0.1, W * 0.9),
        Phaser.Math.FloatBetween(H * 0.2, H * 0.5),
      )
      fish.init(this.snd, randSize())
      fish.setDepth(3)
      fish.setTint(BLOWFISH_TINTS[i % BLOWFISH_TINTS.length])
      this.blowfishes.push(fish)
      this.shakeCtrl.register(fish)
      this.tankCurrent.setFish(fish)
    }

    // ── Bubbles (depth 6 — above all fish) ───────────────────────────────────
    this.bubbleSpawner = new BubbleSpawner(this, this.snd)
    this.shakeCtrl.setBubbleSpawner(this.bubbleSpawner)

    // ── Water currents — 4 horizontal slices ────────────────────────────────
    const sliceH = H / 4
    this.waterCurrents = [0, 1, 2, 3].map(i =>
      new WaterCurrent(0, i * sliceH, W, sliceH, 40, 2, 6)
    )
    for (const c of this.waterCurrents) this.shakeCtrl.registerCurrent(c)

    // ── Tank current (BlowFish horizontal drift) ─────────────────────────────
    this.shakeCtrl.setTankCurrent(this.tankCurrent)

    // ── Hands (depth 2 — behind fish) ────────────────────────────────────────
    if (s.showHands) {
      const rh = this.add.image(0, 0, SPRITES.HANDS_RIGHT).setDepth(2).setOrigin(1, 1)
      const lh = this.add.image(0, 0, SPRITES.HANDS_LEFT).setDepth(2).setOrigin(0, 1)
      const handScale = (W * 0.42) / rh.width
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
    window.addEventListener('rattler:overlay-open',  this.onOverlayOpen)
    window.addEventListener('rattler:overlay-close', this.onOverlayClose)

    this.events.once(Phaser.Scenes.Events.SHUTDOWN, () => {
      window.removeEventListener('rattler:settings-applied', this.onSettingsApplied)
      window.removeEventListener('rattler:overlay-open',  this.onOverlayOpen)
      window.removeEventListener('rattler:overlay-close', this.onOverlayClose)
      this.accel.stop()
    })

    // ── Bubble tap: handled at scene level so bubbles don't block fish taps ──
    // Bubbles are not setInteractive; we find the topmost bubble at the pointer
    // position manually and pop it. Fish receive their own pointerdown events
    // independently since they are the topmost *interactive* objects Phaser sees.
    this.input.on('pointerdown', (pointer: Phaser.Input.Pointer) => {
      let topBubble: import('../objects/Bubble').Bubble | null = null
      let topDepth = -Infinity
      for (const b of this.bubbleSpawner.bubbles) {
        if (!b.active) continue
        const hw = b.displayWidth / 2
        const hh = b.displayHeight / 2
        if (
          pointer.x >= b.x - hw && pointer.x <= b.x + hw &&
          pointer.y >= b.y - hh && pointer.y <= b.y + hh &&
          b.depth > topDepth
        ) {
          topBubble = b
          topDepth = b.depth
        }
      }
      topBubble?.pop()
    })

    this.cameras.main.fadeIn(350, 0, 0, 0)
    this.accel.start()
  }

  update(_time: number, delta: number) {
    if (this.spaceKey && Phaser.Input.Keyboard.JustDown(this.spaceKey)) {
      this.accel.simulateShake()
    }

    for (const g of this.guppies)    g.fishUpdate(delta)
    for (const s of this.starfishes) s.fishUpdate(delta)
    for (const b of this.blowfishes) b.fishUpdate(delta)
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
        fontFamily: "'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif",
      }).setOrigin(0.5).setDepth(11)
      return img
    }

    // Shake button
    const shakeBtn = makeBtn(36, '〜')
    shakeBtn.on('pointerup', () => this.accel.simulateShake())
    shakeBtn.on('pointerover', () => shakeBtn.setAlpha(0.8))
    shakeBtn.on('pointerout',  () => shakeBtn.setAlpha(0.5))

    // Rotate button — toggles landscape/portrait and restarts scene
    const rotBtn = makeBtn(96, '↺')
    rotBtn.on('pointerup', () => {
      const store = useSettingsStore()
      store.set('landscapeMode', !store.settings.landscapeMode)
      window.dispatchEvent(new Event('rattler:settings-applied'))
    })
    rotBtn.on('pointerover', () => rotBtn.setAlpha(0.8))
    rotBtn.on('pointerout',  () => rotBtn.setAlpha(0.5))
  }
}
