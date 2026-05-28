<template>
  <div id="game-container" ref="gameContainer"></div>
  <div id="ui-layer">
    <AboutModal
      v-if="showAbout"
      @close="closeAbout"
    />
    <OptionsMenu
      v-else-if="showOptions"
      @close="showOptions = false"
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

const store = useSettingsStore()

const openOptions = () => { showOptions.value = true }
const openAbout  = () => { showAbout.value = true }

function closeAbout() {
  store.set('firstRun', false)
  showAbout.value = false
}

onMounted(() => {
  createGame(gameContainer.value!)
  window.addEventListener('rattler:open-options', openOptions)
  window.addEventListener('rattler:open-about',   openAbout)
})

onUnmounted(() => {
  window.removeEventListener('rattler:open-options', openOptions)
  window.removeEventListener('rattler:open-about',   openAbout)
})
</script>
