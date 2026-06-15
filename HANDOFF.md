# Rattler — Agent Handoff Brief

> **Read this first.** It's the single source of truth for picking up this project
> cold, in any environment:
> - **New Claude Code session (here):** this file is your onboarding — read it at
>   the start of every session before touching code.
> - **GitHub Copilot (Opus) or elsewhere:** say *"Read HANDOFF.md and act as the
>   agent it describes,"* or paste this file in as your first message.
>
> **⚠️ This file is living documentation. Whenever you change the codebase, update
> the relevant section here in the same change before you publish. See §0.**

---

## 0. Maintaining this file (do not skip)

This brief only stays useful if it stays accurate. The standing rule:

- **Any code change ⇒ update HANDOFF.md in the same branch/commit/PR.** If you
  fix a bug, add the cause + fix to §5. If you change architecture, depths,
  defaults, the event bridge, or the build/ship flow, update §4 / §6. If you
  finish or open a work area, update §7.
- Keep it concise — this is a map and a list of landmines, not a changelog.
  Prune stale entries; "recently fixed" items can age out once they're settled.
- The build/ship loop in §6 is not done until this file reflects the change.

## 1. How to behave

You are working on **Rattler**, a baby-toy game. Whatever environment you're in,
these standards apply:

- **Explain root causes, not just symptoms.** The human values clear "why this
  happened" writeups (e.g. depth/z-order bugs, input timing races). Lead with the
  diagnosis, then the fix.
- **Verify before claiming done.** Always run `npm run build` (tsc + vite) in
  `web/` before saying a change works. Never report success on an unbuilt change.
- **One concern per branch/PR.** Small, focused, descriptive commits.
- **Do not invent APIs.** This is Phaser 3 + Vue 3 + Pinia. Check existing files
  for the pattern before writing new code; match surrounding style.
- **Branch naming:** meaningful names like `fix/bubble-blocking-input`,
  `feature/x`. Do NOT put "claude" or model IDs in branch names, commits, PRs,
  or code comments.
- **Never commit secrets.** (There is a PAT that must never land in the repo.)

## 2. What the project is

Originally a **Unity** C# baby toy. Being ported to a **web SPA** that also ships
to mobile via Capacitor and desktop via Electron. A baby taps fish (they react),
taps bubbles (they pop), and shakes the device (water churns, bubbles burst, fish
get startled). Must be safe: no ads, no IAP, no data collection.

- Repo: `2naturedigital/BabyToy`, default branch `main`.
- The live/active codebase is **`web/`**. The Unity `Assets/`, `ProjectSettings/`
  are the original source of truth for *art and animation timing* — consult the
  `.anim` files when reproducing a creature's behavior, but don't build Unity.

## 3. Tech stack & where things live

```
web/
  package.json        scripts: dev | build | preview | cap:sync
  vite.config.ts      Vite 5 build; BASE_URL drives asset paths
  src/
    main.ts           Vue bootstrap (Pinia)
    App.vue           Mounts Phaser + the Vue overlay layer (modals)
    style.css         @font-face 'Comic Andy'; #ui-layer pointer-events rules
    store/
      settings.ts     Pinia store + ALL defaults + localStorage persistence
    ui/components/
      OptionsMenu.vue  Settings panel (Basic + Advanced tabs)
      AboutModal.vue
    game/
      index.ts         createGame(): Phaser.Game config (540x960 portrait base)
      constants/assets.ts   SPRITES / AUDIO / ANIM key enums
      scenes/
        BootScene.ts        loads sprites+audio, defines ALL animations
        MenuScene.ts        title, Play/Settings/About buttons
        FishTankScene.ts    the actual game; wires every system together
      fish/
        FishController.ts   abstract base: movement, shake boost + wind-down
        Guppy.ts  Starfish.ts  BlowFish.ts
      objects/
        Bubble.ts           single bubble (NOT interactive — see §5)
        BubbleSpawner.ts    spawn cadence, shake bursts, 30-bubble cap
      systems/
        Accelerometer.ts    DeviceMotion -> shake; simulateShake() for desktop
        ShakeController.ts   fan-out broadcaster to fish/currents/spawner
        WaterCurrent.ts      horizontal bubble drift (4 slices)
        TankCurrent.ts       slow horizontal drift for BlowFish only
        SoundController.ts
```

Run locally: `cd web && npm install && npm run dev`. Build check: `npm run build`.

## 4. Critical architecture facts (don't relearn these the hard way)

- **Depth/z-order layers:** Hands=2, BlowFish=3, Guppy=4, Starfish=5. Bubbles
  pick depth from pool `[2,4,7,7]` (≈50% in front of all fish at depth 7).
- **Phaser input is `topOnly`:** only the single topmost *interactive* object at a
  tap point gets the event. This caused the big bug in §5.
- **Two input layers exist and must stay in sync:** the Phaser `<canvas>` and the
  Vue `#ui-layer` overlay. When a modal opens we both (a) set CSS
  `pointer-events:none` on the canvas AND (b) fire `rattler:overlay-open` so
  scenes set `this.input.enabled = false`. On close we wait 500ms then unblock
  both. CSS alone is NOT enough (Vue's async DOM update leaves a 1-frame gap, and
  some mobile browsers deliver touches to `window`, bypassing the canvas rule).
- **Custom window events are the Vue↔Phaser bridge:**
  `rattler:open-options`, `rattler:open-about`, `rattler:settings-applied`
  (triggers `scene.restart()`), `rattler:overlay-open`, `rattler:overlay-close`.
- **Settings flow:** change a slider → Pinia store → on "Done" fire
  `settings-applied` → FishTankScene restarts and re-reads settings. Most settings
  only take effect on restart.
- **Shake intensity is deliberately decoupled** from effect magnitude. Do NOT
  reintroduce `magnitudeMult = ax²+ay²` — that squared term made fish 10x too
  fast. Fish use a fixed 1.5x boost; currents cap at 1.8-2.0x via
  `sqrt(magnitude)`; everything winds down to 1.0 over ~2s after a shake ends;
  bubble horizontal velocity is clamped to ±150 px/s.

## 5. Recently fixed — don't regress these

- **Fish became untappable after shaking** (root cause: bubbles were
  `setInteractive()`, so depth-7 bubbles stole every tap from fish below).
  Fix: bubbles are NOT interactive; `FishTankScene` has a scene-level
  `pointerdown` that finds the topmost bubble at the pointer and pops it. Fish
  keep their own interactivity. `BubbleSpawner` caps at 30 live bubbles.
- **Settings click-through to "Play"** — fixed via the dual-layer block in §4.
- **Starfish blink** is timer-based (every 4–8s) from a resting half-closed
  frame (`starfish_1`); tap/shake opens eyes wide (`starfish_5`), then recovers.
- **Bubble frequency slider**: higher = more bubbles (`interval = 10/freq`).
- **Blowfish baseScale 0.45** (bigger than guppy). **fishSizeVariation default 0.**

## 6. How to ship a change (the loop the human expects)

1. `git checkout -b <meaningful-name>`
2. Make the change. Match existing style.
3. **Update this file (HANDOFF.md)** to reflect the change — see §0.
4. `cd web && npm run build` — must pass clean.
5. Commit with a clear message explaining *what and why* (include the doc update).
6. Open a PR to `main` with a body that states root cause + fix.
7. The human typically wants it **merged to main** (squash). Rebase on
   `origin/main` first if there are conflicts.

> Note on Copilot/other environments: the assistant there may not run the full
> git/PR/merge loop autonomously — it tends to edit-and-suggest. In that case the
> human runs the git commands while the assistant produces the diffs, the commit
> message, and the HANDOFF.md update.

## 7. Open / likely-next areas (not yet done)

- Performance: the JS bundle is ~1.6MB (Phaser). Code-splitting not done.
- No automated tests exist yet — verification is build + manual.
- Mobile (Capacitor) and Electron wrappers exist but the recent gameplay polish
  has only been validated in the web build.
- Watch for any other place assuming the old squared shake magnitude.

## 8. Things to ask the human if unsure

- Whether a change should land on `main` directly or stay as a PR.
- Any change to art/animation *timing* — confirm against the Unity `.anim`
  intent rather than guessing.
