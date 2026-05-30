<template>
  <div class="overlay">
    <div class="panel">

      <div class="fish-row">
        <img :src="`${base}assets/sprites/fish/guppy_swim1.png`" class="fish-icon" alt="guppy" />
        <img :src="`${base}assets/sprites/starfish/starfish1_v2.png`" class="fish-icon" alt="starfish" />
        <img :src="`${base}assets/sprites/blowfish/blowfish4f.png`" class="fish-icon" alt="blowfish" />
      </div>

      <h1>Rattler</h1>
      <p class="tagline">A safe, ad-free baby toy</p>

      <div class="divider" />

      <ul class="features">
        <li>✓ Touch and shake to interact with the fish</li>
        <li>✓ Tap bubbles to pop them</li>
        <li>✓ No ads, no in-app purchases</li>
        <li>✓ No data collected</li>
      </ul>

      <div class="divider" />

      <p class="credit">Made with ❤️ by <strong>2Nature Digital</strong></p>
      <p class="credit small">Richard Moore &amp; James Cotellesse</p>

      <div class="links">
        <a href="mailto:support@2naturedigital.com" class="link">Contact Us</a>
        <span class="dot">·</span>
        <a href="https://2naturedigital.com/privacy" target="_blank" rel="noopener" class="link">Privacy</a>
      </div>

      <button class="got-it" @click="$emit('close')">Got It!</button>

    </div>
  </div>
</template>

<script setup lang="ts">
defineEmits(['close'])
const base = import.meta.env.BASE_URL
</script>

<style scoped>
.overlay {
  position: absolute;
  inset: 0;
  background: rgba(0,10,26,0.88);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 200;
}

.panel {
  background: #0a1f33;
  border: 2px solid #2a6a9e;
  border-radius: 24px;
  width: min(92%, 400px);
  padding: 32px 28px;
  text-align: center;
  font-family: 'Comic Andy', 'Arial Rounded MT Bold', Arial, sans-serif;
  color: #fff;
}

.fish-row {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin-bottom: 12px;
}

.fish-icon {
  height: 52px;
  width: auto;
  object-fit: contain;
}

h1 {
  font-size: 40px;
  color: #4fc3f7;
  margin: 0 0 4px;
}

.tagline {
  font-size: 16px;
  color: #4a7a9b;
  margin: 0 0 20px;
}

.divider {
  height: 1px;
  background: #1a3a55;
  margin: 16px 0;
}

.features {
  list-style: none;
  padding: 0;
  margin: 0;
  text-align: left;
}

.features li {
  font-size: 16px;
  color: #cce6ff;
  padding: 6px 0;
}

.credit {
  font-size: 15px;
  color: #4a7a9b;
  margin: 4px 0;
}

.credit.small { font-size: 13px; }

.links {
  margin: 14px 0 20px;
  font-size: 14px;
}

.link {
  color: #4fc3f7;
  text-decoration: none;
}

.dot { color: #2a6a9e; margin: 0 8px; }

.got-it {
  width: 100%;
  padding: 16px;
  background: #0077cc;
  border: none;
  border-radius: 14px;
  color: #fff;
  font-size: 22px;
  font-weight: bold;
  cursor: pointer;
  font-family: inherit;
}

.got-it:active { background: #005fa3; }
</style>
