const { app, BrowserWindow } = require('electron')
const http = require('http')
const fs = require('fs')
const path = require('path')

// Packaged: resources/app/ holds the web dist.  Dev: ../web/dist relative to this file.
const DIST_DIR = app.isPackaged
  ? path.join(process.resourcesPath, 'app')
  : path.join(__dirname, '../web/dist')

const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.js':   'text/javascript',
  '.css':  'text/css',
  '.png':  'image/png',
  '.jpg':  'image/jpeg',
  '.gif':  'image/gif',
  '.svg':  'image/svg+xml',
  '.wav':  'audio/wav',
  '.mp3':  'audio/mpeg',
  '.ogg':  'audio/ogg',
  '.json': 'application/json',
  '.ico':  'image/x-icon',
  '.woff2':'font/woff2',
}

// Spin up a local HTTP server so absolute paths like /assets/... work correctly.
// Listens only on 127.0.0.1 — not exposed to the network.
function startServer(onReady) {
  const server = http.createServer((req, res) => {
    const urlPath = req.url.split('?')[0]
    let filePath = path.join(DIST_DIR, urlPath === '/' ? 'index.html' : urlPath)

    if (!fs.existsSync(filePath) || fs.statSync(filePath).isDirectory()) {
      // SPA fallback — let Vue Router handle the route
      filePath = path.join(DIST_DIR, 'index.html')
    }

    const ext = path.extname(filePath).toLowerCase()
    res.writeHead(200, { 'Content-Type': MIME[ext] || 'application/octet-stream' })
    fs.createReadStream(filePath).pipe(res)
  })

  server.listen(0, '127.0.0.1', () => onReady(server.address().port))
}

function createWindow(port) {
  const win = new BrowserWindow({
    width: 540,
    height: 960,
    title: 'Rattler',
    backgroundColor: '#000820',
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
    },
  })

  win.loadURL(`http://127.0.0.1:${port}`)

  // Remove default menu bar (not needed for a game)
  win.setMenuBarVisibility(false)

  win.on('closed', () => app.quit())
}

app.whenReady().then(() => {
  startServer((port) => createWindow(port))
})

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') app.quit()
})

app.on('activate', () => {
  // macOS: re-create window when dock icon is clicked
  if (BrowserWindow.getAllWindows().length === 0) {
    startServer((port) => createWindow(port))
  }
})
