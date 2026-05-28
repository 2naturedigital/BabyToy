<template>
  <div class="overlay" @click.self="close">
    <div class="panel">

      <div class="panel-header">
        <h2>Settings</h2>
        <button class="x-btn" @click="close">✕</button>
      </div>

      <div class="tabs">
        <button :class="['tab', { active: tab === 'basic' }]" @click="tab = 'basic'">Basic</button>
        <button :class="['tab', { active: tab === 'advanced' }]" @click="tab = 'advanced'">Advanced</button>
      </div>

      <div class="content">

        <!-- ── BASIC TAB ── -->
        <template v-if="tab === 'basic'">

          <div class="section-label">Sound</div>
          <Row label="Volume" :value="pct(s.volume)">
            <input type="range" min="0" max="1" step="0.05"
              :value="s.volume" @input="set('volume', num($event))" />
          </Row>

          <div class="section-label">Bubbles</div>
          <Row label="Frequency" :value="freqLabel(s.bubbleFrequency)">
            <input type="range" min="1" max="8" step="0.5"
              :value="s.bubbleFrequency" @input="set('bubbleFrequency', num($event))" />
          </Row>
          <Row label="Size" :value="sizeLabel(s.bubbleSize)">
            <input type="range" min="0.5" max="2" step="0.1"
              :value="s.bubbleSize" @input="set('bubbleSize', num($event))" />
          </Row>

          <div class="section-label">Fish</div>
          <Row label="All Fish Size" :value="sizeLabel(masterSize)">
            <input type="range" min="0.5" max="2" step="0.1"
              :value="masterSize" @input="setMasterSize(num($event))" />
          </Row>

          <div class="section-label">Controls</div>
          <Row label="Shake Sensitivity" :value="sizeLabel(s.shakeSensitivity)">
            <input type="range" min="0.5" max="2.5" step="0.1"
              :value="s.shakeSensitivity" @input="set('shakeSensitivity', num($event))" />
          </Row>

          <div class="section-label">Display</div>
          <ToggleRow label="Show Hands" :value="s.showHands"
            @toggle="set('showHands', !s.showHands)" />
          <ToggleRow label="Water Overlay" :value="s.showWaterLayer"
            @toggle="set('showWaterLayer', !s.showWaterLayer)" />
          <ToggleRow label="Blur Background" :value="s.blurBackground"
            @toggle="set('blurBackground', !s.blurBackground)" />

        </template>

        <!-- ── ADVANCED TAB ── -->
        <template v-else>

          <div class="section-label">Individual Fish Sizes</div>
          <Row label="Guppy" :value="sizeLabel(s.guppySize)">
            <input type="range" min="0.5" max="2" step="0.1"
              :value="s.guppySize" @input="set('guppySize', num($event))" />
          </Row>
          <Row label="Starfish" :value="sizeLabel(s.starfishSize)">
            <input type="range" min="0.5" max="2" step="0.1"
              :value="s.starfishSize" @input="set('starfishSize', num($event))" />
          </Row>
          <Row label="Blowfish" :value="sizeLabel(s.blowfishSize)">
            <input type="range" min="0.5" max="2" step="0.1"
              :value="s.blowfishSize" @input="set('blowfishSize', num($event))" />
          </Row>

          <div class="section-label">Shake</div>
          <Row label="Shake Power" :value="sizeLabel(s.shakePower)">
            <input type="range" min="0.5" max="3" step="0.1"
              :value="s.shakePower" @input="set('shakePower', num($event))" />
          </Row>

          <div class="section-label">Bubble Fine-tuning</div>
          <Row label="Bubbles per Shake" :value="String(s.bubbleCount)">
            <input type="range" min="1" max="5" step="1"
              :value="s.bubbleCount" @input="set('bubbleCount', num($event))" />
          </Row>
          <Row label="Size Variation" :value="pct(s.bubbleSizeVariation)">
            <input type="range" min="0" max="1" step="0.05"
              :value="s.bubbleSizeVariation" @input="set('bubbleSizeVariation', num($event))" />
          </Row>

          <div class="reset-wrap">
            <button class="reset-btn" @click="resetAll">Reset All to Defaults</button>
          </div>

        </template>

      </div><!-- /content -->

      <div class="footer-note">Tap Done to apply changes and restart.</div>
      <button class="done-btn" @click="close">Done</button>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useSettingsStore } from '../../store/settings'

// Sub-components inlined as render helpers
const Row = {
  props: ['label', 'value'],
  template: `
    <div class="row">
      <div class="row-top">
        <span class="row-label">{{ label }}</span>
        <span class="row-value">{{ value }}</span>
      </div>
      <slot />
    </div>
  `,
}

const ToggleRow = {
  props: ['label', 'value'],
  emits: ['toggle'],
  template: `
    <div class="row toggle-row" @click="$emit('toggle')">
      <span class="row-label">{{ label }}</span>
      <div :class="['toggle', { on: value }]"><div class="thumb" /></div>
    </div>
  `,
}

const emit = defineEmits(['close'])
const store = useSettingsStore()
const s = computed(() => store.settings)
const tab = ref<'basic' | 'advanced'>('basic')

function num(e: Event) { return Number((e.target as HTMLInputElement).value) }
function set<K extends keyof typeof s.value>(key: K, value: typeof s.value[K]) {
  store.set(key, value)
}

const masterSize = computed(() =>
  (s.value.guppySize + s.value.starfishSize + s.value.blowfishSize) / 3
)
function setMasterSize(v: number) {
  store.set('guppySize', v)
  store.set('starfishSize', v)
  store.set('blowfishSize', v)
}

function pct(v: number) { return `${Math.round(v * 100)}%` }

function sizeLabel(v: number) {
  if (v < 0.75) return 'Small'
  if (v < 1.15) return 'Normal'
  if (v < 1.55) return 'Big'
  return 'Huge'
}

function freqLabel(v: number) {
  if (v <= 2) return 'Lots'
  if (v <= 4) return 'Normal'
  if (v <= 6) return 'Few'
  return 'Rare'
}

function resetAll() {
  const d = {
    volume: 1.0, bubbleFrequency: 4, bubbleSize: 1.0, bubbleCount: 2,
    bubbleSizeVariation: 0.5, guppySize: 1.0, starfishSize: 1.0, blowfishSize: 1.0,
    shakeSensitivity: 1.0, shakePower: 1.0, shakenBubbleFrequency: 0.13,
    showHands: true, showWaterLayer: true, blurBackground: false,
  }
  for (const [k, v] of Object.entries(d)) {
    store.set(k as keyof typeof s.value, v as never)
  }
}

function close() {
  window.dispatchEvent(new Event('rattler:settings-applied'))
  emit('close')
}
</script>

<style scoped>
.overlay {
  position: absolute;
  inset: 0;
  background: rgba(0,0,0,0.72);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.panel {
  background: #0a1f33;
  border: 2px solid #2a6a9e;
  border-radius: 20px;
  width: min(94%, 440px);
  max-height: 88vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  font-family: Arial Rounded MT Bold, Arial, sans-serif;
  color: #fff;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 20px 8px;
}

.panel-header h2 {
  font-size: 26px;
  color: #4fc3f7;
  margin: 0;
}

.x-btn {
  background: none;
  border: none;
  color: #4a7a9b;
  font-size: 22px;
  cursor: pointer;
  padding: 4px 8px;
}

.tabs {
  display: flex;
  margin: 0 16px 4px;
  border-bottom: 2px solid #1a3a55;
}

.tab {
  flex: 1;
  background: none;
  border: none;
  color: #4a7a9b;
  font-size: 17px;
  padding: 10px 0;
  cursor: pointer;
  font-family: inherit;
  border-bottom: 3px solid transparent;
  margin-bottom: -2px;
}

.tab.active {
  color: #4fc3f7;
  border-bottom-color: #4fc3f7;
}

.content {
  overflow-y: auto;
  padding: 10px 16px 4px;
  flex: 1;
  -webkit-overflow-scrolling: touch;
}

.section-label {
  font-size: 12px;
  color: #2a6a9e;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin: 14px 0 6px;
}

.row {
  margin-bottom: 10px;
}

.row-top {
  display: flex;
  justify-content: space-between;
  margin-bottom: 4px;
}

.row-label { font-size: 16px; color: #cce6ff; }
.row-value  { font-size: 16px; color: #4fc3f7; min-width: 52px; text-align: right; }

input[type="range"] {
  width: 100%;
  height: 6px;
  accent-color: #4fc3f7;
  cursor: pointer;
}

.toggle-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
  cursor: pointer;
  border-bottom: 1px solid #0d2b44;
}

.toggle {
  width: 46px;
  height: 26px;
  border-radius: 13px;
  background: #1a3a55;
  position: relative;
  transition: background 0.2s;
  flex-shrink: 0;
}

.toggle.on { background: #0077cc; }

.thumb {
  position: absolute;
  top: 3px;
  left: 3px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: #fff;
  transition: left 0.2s;
}

.toggle.on .thumb { left: 23px; }

.reset-wrap {
  margin-top: 20px;
  text-align: center;
}

.reset-btn {
  background: none;
  border: 1px solid #2a6a9e;
  color: #4a7a9b;
  font-size: 14px;
  padding: 8px 20px;
  border-radius: 8px;
  cursor: pointer;
  font-family: inherit;
}

.footer-note {
  font-size: 12px;
  color: #2a5a7a;
  text-align: center;
  padding: 8px 16px 4px;
}

.done-btn {
  display: block;
  width: calc(100% - 32px);
  margin: 10px 16px 16px;
  padding: 16px;
  background: #0077cc;
  border: none;
  border-radius: 12px;
  color: #fff;
  font-size: 20px;
  font-weight: bold;
  cursor: pointer;
  font-family: inherit;
  flex-shrink: 0;
}

.done-btn:active { background: #005fa3; }
</style>
