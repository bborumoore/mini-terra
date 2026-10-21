# Project Terra — status and decisions

Updated 2026-10-21. This file supplements, rather than rewrites, the [historical discovery dossier](reference/PROJECT_TERRA_BUILD_BIBLE_DISCOVERY_V0_1.md).

## Confirmed decisions
| ID | Decision | Source |
| --- | --- | --- |
| DEC-001 | Use bborumoore/mini-terra for Project Terra. | Brian's 2026-10-21 request |
| DEC-002 | Target Unity. Exact editor version is open. | Same request |
| DEC-003 | Use issues, feature branches and pull requests. | Same request |

The dossier's repository-target uncertainty is resolved for this work. Its historical game design remains unconfirmed. No mechanics from Totem Clash or other projects are transferable.

## Current implementation
Main at inspection: 755d57221536a0672b5051aec6983a82f1b59883, containing README.md and .gitignore only. This foundation PR adds documentation and templates, not Unity scenes or gameplay. No Unity build, test run, or human playtest has occurred.

## Questions for Brian
1. Is there a newer completed BUILD_BIBLE.md or a local Unity project? This attachment explicitly calls itself a discovery draft.
2. What does the player control, repeatedly do, and try to accomplish? Describe one concrete obstacle and solution, ideally a first 3–5 minute session.
3. What perspective should we use: 2D side view, 2D top-down, 3D first-person, 3D third-person, or another?
4. What causes failure, and what does retry/reset restore?
5. Is Windows with keyboard/mouse the first target? Which Unity version is installed, and can Brian run editor tests/builds locally?

## Dependency-ordered roadmap
| Issue | Deliverable | Dependency | Current status |
| --- | --- | --- | --- |
| [#1](https://github.com/bborumoore/mini-terra/issues/1) | Approved first-playable specification | Brian's design answers | Open |
| [#2](https://github.com/bborumoore/mini-terra/issues/2) | Development workflow and source record | None | Prepared for review |
| [#3](https://github.com/bborumoore/mini-terra/issues/3) | Verified Unity project and build path | #1 technical decisions | Blocked |
| [#4](https://github.com/bborumoore/mini-terra/issues/4) | Implemented and playtested vertical slice | #1, #3 | Blocked |

After #1, create focused mechanic issues using stable rule IDs and success/invalid-action/reset examples. Do not turn the illustrative two-puzzle template into an approved design. Use the latest explicit decisions as authority and date every change.

## Definition of playable
A clean checkout builds with the pinned editor; the resulting artifact launches on the agreed platform; the approved controls, core loop, success and recovery paths work; automated tests have actual results; Brian has completed the manual acceptance playthrough. Record exact commit and remaining defects. Documentation or source presence alone does not meet this definition.
