<template>
  <!-- canvasBlocked is decoupled from show state so we can keep it true
       for a brief period after a modal closes, preventing the closing
       touch/click from propagating through to Phaser. -->
  <div id="game-container" ref="gameContainer"
       :class="{ 'game-blocked': canvasBlocked }"></div>
  <div id="ui-layer">
    <AboutModal
      v-if="showAbout"
      @close="closeAbout"
    />
    <OptionsMenu
      v-else-if="showOptions"
      @close="closeOptions"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import OptionsMenu from './ui/components/OptionsMenu.vue'
import AboutModal from './ui/components/AboutModal.vue'
import { createGame } from './game/index'
import { useSettingsStore } from './store/settings'

const gameContainer = ref<HTMLElement | null>(null)
const showOptions = ref(false)
const showAbout = ref(false)
const canvasBlocked = ref(false)  // controls pointer-events independently

const store = useSettingsStore()

const openOptions = () => { showOptions.value = true;  canvasBlocked.value = true }
const openAbout   = () => { showAbout.value  = true;   canvasBlocked.value = true }

// Keep canvas blocked for 500 ms after the modal disappears so the closing
// touch/pointer-up cannot fall through to Phaser buttons behind the overlay.
// 500ms covers the 300ms mobile ghost-click delay with margin.
function unblockCanvas() {
  setTimeout(() => { canvasBlocked.value = false }, 500)
}

function closeAbout() {
  store.set('firstRun', false)
  showAbout.value = false
  unblockCanvas()
}

function closeOptions() {
  showOptions.value = false
  unblockCanvas()
}

onMounted(async () => {
  // Ensure Comic Andy is loaded before Phaser canvas text renders it
  await document.fonts.load("16px 'Comic Andy'").catch(() => {/* fallback font ok */})
  createGame(gameContainer.value!)
  window.addEventListener('rattler:open-options', openOptions)
  window.addEventListener('rattler:open-about',   openAbout)
})

onUnmounted(() => {
  window.removeEventListener('rattler:open-options', openOptions)
  window.removeEventListener('rattler:open-about',   openAbout)
})
</script>

<style>
#game-container.game-blocked canvas {
  pointer-events: none;
}
</style>
