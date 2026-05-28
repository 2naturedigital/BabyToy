import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  // VITE_BASE_PATH lets CI/Electron set the correct base without code changes:
  //   GitHub Pages: VITE_BASE_PATH=/BabyToy/
  //   Electron:     VITE_BASE_PATH=./
  //   Default:      /
  base: process.env.VITE_BASE_PATH ?? '/',
  define: {
    global: 'globalThis',
  },
  server: {
    port: 5173,
    host: true,
  },
  build: {
    outDir: 'dist',
    target: 'es2020',
  },
})
