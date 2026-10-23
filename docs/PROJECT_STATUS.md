# Project Terra — status and decisions

Updated 2026-10-22. Current design authority is [BUILD_BIBLE.md](../BUILD_BIBLE.md); the [discovery dossier](reference/PROJECT_TERRA_BUILD_BIBLE_DISCOVERY_V0_1.md) remains historical reference.

## Confirmed foundation

Fresh Unity 6000.5.9f1 implementation, 2D side view, Windows keyboard/mouse first. Controller and Mac/Linux later. Precision boss combat, no puzzles, minimal exploration, no current NPCs. First playable is a village plus one boss. Pixel art/dark palette.

## Review sequence

1. [PR #5](https://github.com/bborumoore/mini-terra/pull/5) contains the development workflow; it is still open.
2. Branch `feat/6-first-combat` is stacked on that foundation to avoid duplicating or losing its documents. Its PR targets the foundation branch until #5 merges, then can be retargeted to main.
3. [Issue #6](https://github.com/bborumoore/mini-terra/issues/6) tracks the first combat prototype. The feature PR supplies source, tests, scene and build entry point, but does not close acceptance before Unity/build/playtest evidence exists.

## Backlog

| Issue | Status |
| --- | --- |
| [#1 Design](https://github.com/bborumoore/mini-terra/issues/1) | Core vision and first-playable scope specified; campaign ambiguities recorded in bible. |
| [#2 Workflow](https://github.com/bborumoore/mini-terra/issues/2) | Foundation PR #5 awaiting review. |
| [#3 Unity](https://github.com/bborumoore/mini-terra/issues/3) | Source scaffold prepared; actual Unity import and build still required. |
| [#4 First playable](https://github.com/bborumoore/mini-terra/issues/4) | Prototype implementation under #6; human feel/balance acceptance pending. |
| [#6 First combat](https://github.com/bborumoore/mini-terra/issues/6) | 18 headless specifications pass; Unity and Windows verification pending. |

## Next design decisions

Death persistence for buildings/gear/unlocks; mixed boss seed rewards; automatic versus manual seed pickup; elemental bonus timing/order; cardinal/diagonal map and traversal unlocks; depleted-element behavior; final-boss location; save/quit rules. These do not block testing the isolated first fight.

## Status vocabulary

Implemented source is distinct from compiled in Unity, tested in Unity, built, playtested, and accepted. See [VALIDATION.md](VALIDATION.md) for actual evidence and [PLAYTEST.md](PLAYTEST.md) for remaining gates.
