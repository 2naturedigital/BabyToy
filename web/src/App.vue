<template>
  <div id="game-container" ref="gameContainer"></div>
  <div id="ui-layer">
    <OptionsMenu v-if="showOptions" @close="showOptions = false" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import OptionsMenu from './ui/components/OptionsMenu.vue'
import { createGame } from './game/index'

const gameContainer = ref<HTMLElement | null>(null)
const showOptions = ref(false)

const handleOpenOptions = () => { showOptions.value = true }

onMounted(() => {
  createGame(gameContainer.value!)
  window.addEventListener('rattler:open-options', handleOpenOptions)
})

onUnmounted(() => {
  window.removeEventListener('rattler:open-options', handleOpenOptions)
})
</script>
