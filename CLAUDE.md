# CLAUDE.md

**Start here:** Read [`HANDOFF.md`](./HANDOFF.md) before doing anything in this
repo. It is the canonical onboarding + architecture brief for the Rattler project.

## Standing rules

- **Keep HANDOFF.md current.** Whenever you change the codebase, update the
  relevant section of HANDOFF.md in the *same* branch/commit/PR. A code change is
  not "done" until the doc reflects it. (Details in HANDOFF.md §0.)
- The active codebase is `web/` (Phaser 3 + Vue 3 + Pinia + TypeScript/Vite).
- Verify with `cd web && npm run build` before claiming a change works.
- Meaningful branch names; no model names/IDs or secrets in branches, commits,
  PRs, or code.
