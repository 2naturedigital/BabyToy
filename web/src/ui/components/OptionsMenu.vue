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
          <div class="row">
            <div class="row-top">
              <span class="row-label">Volume</span>
              <span class="row-value">{{ pct(s.volume) }}</span>
            </div>
            <input type="range" min="0" max="1" step="0.05"
              :value="s.volume" @input="set('volume', num($event))" />
          </div>

          <div class="section-label">Bubbles</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Frequency</span>
              <span class="row-value">{{ freqLabel(s.bubbleFrequency) }}</span>
            </div>
            <input type="range" min="1" max="8" step="0.5"
              :value="s.bubbleFrequency" @input="set('bubbleFrequency', num($event))" />
          </div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Size</span>
              <span class="row-value">{{ sizeLabel(s.bubbleSize) }}</span>
            </div>
            <input type="range" min="0.5" max="3" step="0.1"
              :value="s.bubbleSize" @input="set('bubbleSize', num($event))" />
          </div>

          <div class="section-label">Fish</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">All Fish Size</span>
              <span class="row-value">{{ sizeLabel(s.fishSize) }}</span>
            </div>
            <input type="range" min="0.5" max="2" step="0.1"
              :value="s.fishSize" @input="set('fishSize', num($event))" />
          </div>

          <div class="section-label">Controls</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Shake Sensitivity</span>
              <span class="row-value">{{ sizeLabel(s.shakeSensitivity) }}</span>
            </div>
            <input type="range" min="0.5" max="2.5" step="0.1"
              :value="s.shakeSensitivity" @input="set('shakeSensitivity', num($event))" />
          </div>

          <div class="section-label">Display</div>
          <div class="row toggle-row" @click="set('showHands', !s.showHands)">
            <span class="row-label">Show Hands</span>
            <div :class="['toggle', { on: s.showHands }]"><div class="thumb" /></div>
          </div>
          <div class="row toggle-row" @click="set('showWaterLayer', !s.showWaterLayer)">
            <span class="row-label">Water Overlay</span>
            <div :class="['toggle', { on: s.showWaterLayer }]"><div class="thumb" /></div>
          </div>
          <div class="row toggle-row" @click="set('blurBackground', !s.blurBackground)">
            <span class="row-label">Blur Background</span>
            <div :class="['toggle', { on: s.blurBackground }]"><div class="thumb" /></div>
          </div>

        </template>

        <!-- ── ADVANCED TAB ── -->
        <template v-else>

          <div class="section-label">Fish Fine-tuning</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Size Variation</span>
              <span class="row-value">{{ pct(s.fishSizeVariation) }}</span>
            </div>
            <input type="range" min="0" max="0.5" step="0.05"
              :value="s.fishSizeVariation" @input="set('fishSizeVariation', num($event))" />
          </div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Guppy Count</span>
              <span class="row-value">{{ s.guppyCount }}</span>
            </div>
            <input type="range" min="0" max="3" step="1"
              :value="s.guppyCount" @input="set('guppyCount', num($event))" />
          </div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Starfish Count</span>
              <span class="row-value">{{ s.starfishCount }}</span>
            </div>
            <input type="range" min="0" max="3" step="1"
              :value="s.starfishCount" @input="set('starfishCount', num($event))" />
          </div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Blowfish Count</span>
              <span class="row-value">{{ s.blowfishCount }}</span>
            </div>
            <input type="range" min="0" max="3" step="1"
              :value="s.blowfishCount" @input="set('blowfishCount', num($event))" />
          </div>

          <div class="section-label">Shake</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Shake Power</span>
              <span class="row-value">{{ sizeLabel(s.shakePower) }}</span>
            </div>
            <input type="range" min="0.5" max="3" step="0.1"
              :value="s.shakePower" @input="set('shakePower', num($event))" />
          </div>

          <div class="section-label">Bubble Fine-tuning</div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Shake Bubble Intensity</span>
              <span class="row-value">{{ shakeIntensityLabel(s.bubbleCount) }}</span>
            </div>
            <input type="range" min="1" max="10" step="1"
              :value="s.bubbleCount" @input="set('bubbleCount', num($event))" />
          </div>
          <div class="row">
            <div class="row-top">
              <span class="row-label">Size Variation</span>
              <span class="row-value">{{ pct(s.bubbleSizeVariation) }}</span>
            </div>
            <input type="range" min="0" max="1" step="0.05"
              :value="s.bubbleSizeVariation" @input="set('bubbleSizeVariation', num($event))" />
          </div>

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
import { ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useSettingsStore } from '../../store/settings'
import type { Settings } from '../../store/settings'

const emit = defineEmits(['close'])
const store = useSettingsStore()
// storeToRefs extracts the settings ref so template auto-unwrapping works reliably.
// Directly accessing store.settings can skip ref-unwrapping in some Pinia v2 builds.
const { settings: s } = storeToRefs(store)
const tab = ref<'basic' | 'advanced'>('basic')

function num(e: Event) { return Number((e.target as HTMLInputElement).value) }
function set<K extends keyof Settings>(key: K, value: Settings[K]) {
  store.set(key, value)
}

function pct(v: number) { return `${Math.round(v * 100)}%` }

function sizeLabel(v: number) {
  if (v < 0.75) return 'Small'
  if (v < 1.15) return 'Normal'
  if (v < 1.55) return 'Big'
  return 'Huge'
}

function freqLabel(v: number) {
  if (v <= 2) return 'Rare'
  if (v <= 4) return 'Few'
  if (v <= 6) return 'Often'
  return 'Lots'
}

function shakeIntensityLabel(v: number) {
  if (v <= 2) return 'Light'
  if (v <= 5) return 'Normal'
  if (v <= 8) return 'Heavy'
  return 'Tsunami'
}

function resetAll() {
  const d: Partial<Settings> = {
    volume: 1.0, bubbleFrequency: 5.0, bubbleSize: 1.0, bubbleCount: 5,
    bubbleSizeVariation: 0.5, fishSize: 1.0, fishSizeVariation: 0.0,
    guppyCount: 1, starfishCount: 1, blowfishCount: 1,
    shakeSensitivity: 1.0, shakePower: 1.0, shakenBubbleFrequency: 0.13,
    showHands: true, showWaterLayer: true, blurBackground: false,
  }
  for (const [k, v] of Object.entries(d)) {
    store.set(k as keyof Settings, v as never)
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
  font-family: 'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif;
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
