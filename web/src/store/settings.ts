import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

const STORAGE_KEY = 'rattler_settings'

const defaults = {
  volume: 1.0,
  bubbleFrequency: 2.0,
  bubbleSize: 1.0,
  bubbleCount: 5,
  bubbleSizeVariation: 0.5,
  guppySize: 1.0,
  starfishSize: 1.0,
  blowfishSize: 1.0,
  shakeSensitivity: 1.0,
  shakePower: 1.0,
  shakenBubbleFrequency: 0.13,
  landscapeMode: false,
  showHands: true,
  showWaterLayer: true,
  blurBackground: false,
  firstRun: true,
}

export type Settings = typeof defaults

function load(): Settings {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    if (raw) return { ...defaults, ...JSON.parse(raw) }
  } catch {
    // ignore
  }
  return { ...defaults }
}

export const useSettingsStore = defineStore('settings', () => {
  const s = ref<Settings>(load())

  watch(s, (val) => {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(val))
  }, { deep: true })

  function set<K extends keyof Settings>(key: K, value: Settings[K]) {
    s.value[key] = value
  }

  function get<K extends keyof Settings>(key: K): Settings[K] {
    return s.value[key]
  }

  return { settings: s, set, get }
})
