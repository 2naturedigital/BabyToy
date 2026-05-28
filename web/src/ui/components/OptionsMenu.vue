<template>
  <div class="overlay" @click.self="$emit('close')">
    <div class="panel">
      <h2>Settings</h2>

      <div class="row">
        <label>Volume</label>
        <input type="range" min="0" max="1" step="0.05"
          :value="store.settings.volume"
          @input="store.set('volume', +($event.target as HTMLInputElement).value)" />
      </div>

      <div class="row">
        <label>Bubble Frequency</label>
        <input type="range" min="1" max="8" step="0.5"
          :value="store.settings.bubbleFrequency"
          @input="store.set('bubbleFrequency', +($event.target as HTMLInputElement).value)" />
      </div>

      <div class="row">
        <label>Bubble Size</label>
        <input type="range" min="0.5" max="2" step="0.1"
          :value="store.settings.bubbleSize"
          @input="store.set('bubbleSize', +($event.target as HTMLInputElement).value)" />
      </div>

      <div class="row">
        <label>Shake Sensitivity</label>
        <input type="range" min="0.5" max="2.5" step="0.1"
          :value="store.settings.shakeSensitivity"
          @input="store.set('shakeSensitivity', +($event.target as HTMLInputElement).value)" />
      </div>

      <div class="row toggle">
        <label>Show Hands</label>
        <input type="checkbox"
          :checked="store.settings.showHands"
          @change="store.set('showHands', ($event.target as HTMLInputElement).checked)" />
      </div>

      <div class="row toggle">
        <label>Blur Background</label>
        <input type="checkbox"
          :checked="store.settings.blurBackground"
          @change="store.set('blurBackground', ($event.target as HTMLInputElement).checked)" />
      </div>

      <button class="close-btn" @click="$emit('close')">Done</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useSettingsStore } from '../../store/settings'
defineEmits(['close'])
const store = useSettingsStore()
</script>

<style scoped>
.overlay {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.75);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.panel {
  background: #0d2b45;
  border: 3px solid #4fc3f7;
  border-radius: 24px;
  padding: 40px;
  width: min(90%, 420px);
  color: #fff;
  font-family: Arial Rounded MT Bold, Arial, sans-serif;
}

h2 {
  font-size: 32px;
  text-align: center;
  margin-bottom: 28px;
  color: #4fc3f7;
}

.row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
  gap: 16px;
}

.row label {
  font-size: 18px;
  white-space: nowrap;
}

.row input[type="range"] {
  flex: 1;
  accent-color: #4fc3f7;
}

.toggle input[type="checkbox"] {
  width: 24px;
  height: 24px;
  accent-color: #4fc3f7;
}

.close-btn {
  display: block;
  width: 100%;
  margin-top: 28px;
  padding: 16px;
  background: #4fc3f7;
  border: none;
  border-radius: 14px;
  color: #0d2b45;
  font-size: 22px;
  font-weight: bold;
  cursor: pointer;
  font-family: inherit;
}

.close-btn:hover {
  background: #81d4fa;
}
</style>
