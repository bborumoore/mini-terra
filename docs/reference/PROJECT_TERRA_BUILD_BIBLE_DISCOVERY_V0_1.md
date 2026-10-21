# PROJECT TERRA — Discovery Dossier & Build Bible (v0.1)

**Status:** Discovery draft / NOT implementation-ready  
**Prepared:** 2026-10-19  
**Purpose:** Gather verifiable prior context and enumerate decisions needed to produce an implementation-grade, agent-handoff MVP build bible.  
**Scope:** Available connected GitHub account, associated repository material, previous contextual discussion retrieval, and accessible file search. No source files were modified.  

> **Critical identity warning:** The only connected repository with “Terra” in its name is **`bborumoore/mini-terra`**. Its content does **not** establish that it is the same project as the requested **Project Terra**. Nothing about Project Terra's mechanics should be treated as canonical until this is confirmed.

## 1. Evidence legend and source authority

- **CONFIRMED-SOURCE** — Verbatim or directly observable in a retrieved source; confirms only what the cited source says, not whether the user's present vision is unchanged.
- **IMPLEMENTED-VERIFIED-BY-REPO** — Existing committed source, asset, test, or scene observed in a repository. Does *not* mean executable behavior has been run or tested.
- **HISTORICAL** — Older project description; must be re-confirmed before being promoted to current design.
- **RELATED, NOT TRANSFERABLE** — Another game/repository belonging to the same account. Do not import mechanics without explicit approval.
- **PROPOSAL** — Suggested design, scope, test, architecture, or process to speed discussion; not user-approved.
- **OPEN** — Cannot be determined from recovered material.

**Proposed source-of-truth order:** (1) latest explicit user decisions, (2) agreed build bible and acceptance tests, (3) target repository code and assets for implementation reality, (4) older repo descriptions, (5) suggestions. Preserve conflicts explicitly; never infer confirmation from an assistant recommendation.

## 2. Source inventory

| ID | Source | Evidence | Interpretation / limits |
|---|---|---|---|
| G-01 | [GitHub: bborumoore/mini-terra](https://github.com/bborumoore/mini-terra) | A public `main` repository with README and Unity `.gitignore`; one recorded initial commit dated 2021-09-06 | Possible ancestor, unconfirmed as Project Terra |
| G-02 | [Mini Terra README](https://github.com/bborumoore/mini-terra/blob/main/README.md) | “My third Unity project, this game aims to be a simple adventure/puzzle game.” | Historical description, not a detailed game design |
| G-03 | [Mini Terra `.gitignore`](https://github.com/bborumoore/mini-terra/blob/main/.gitignore) | Unity-oriented ignore rules | Further evidence this repository was intended for Unity, not proof of a runnable project |
| G-04 | [Portfolio project catalog](https://github.com/bborumoore/web-portfolio/blob/main/src/data/projects.json) | Lists **Mini Terra** as a 2021 **Unity / Adventure Puzzle** project, marked **archived** | Secondary evidence of the historical concept; not current Project Terra confirmation |
| G-05 | [Generated portfolio catalog](https://github.com/bborumoore/web-portfolio/blob/main/src/data/projects.generated.json) | Repeats Mini Terra's description | Duplication, not an independent rule specification |
| G-06 | [Square Hole](https://github.com/bborumoore/square-hole) | Another distinct earlier game/prototype | Related developer history, **not** Project Terra |
| G-07 | [Slime Time](https://github.com/bborumoore/slime-time) and [collaborative implementation](https://github.com/bborumoore/slime-time-repo) | Separate 2D platformer focusing on size manipulation; actual Unity code is visible in collaborative repo | Do **not** assume these mechanics belong to Terra |
| G-08 | [Grav Maga](https://github.com/bborumoore/grav-maga) | Separate platformer concept about switching gravity direction | Do **not** merge into Terra |
| G-09 | [Another Project](https://github.com/bborumoore/another-project) | Basic Unity scaffold/scene, generic README, no documented Terra link | **No evidence** this is Terra's codebase |
| G-10 | [New Game](https://github.com/bborumoore/new-game) | Sparse separate repository described as a prototype refactor | No evidence of identity with Terra |

**Coverage:** Enumerated accessible repositories on the linked GitHub account and searched for Terra-named repositories, owner-specific code references, Terra-related issues and PRs. The only directly matching repository name was `mini-terra`; there were no Terra-specific issues found in that repository. Searched accessible conversation/library context for the project name; no substantive Project Terra specification surfaced. Search indexing/access may omit uncommitted local files, other accounts, or material not shared with this assistant.

## 3. What can actually be asserted today

### 3.1 Historical design seed — CONDITIONAL

If Project Terra is a revival or renaming of Mini Terra, the known historical design description is:

- **Genre/intent:** Simple adventure/puzzle game.
- **Engine:** Unity.
- **Historical dating:** Repo initial commit in September 2021; portfolio labels project 2021.
- **Development status, historically:** Repository is effectively an empty shell, containing `README.md` and `.gitignore` but no committed `Assets/`, `Packages/`, `ProjectSettings/`, scenes, C# scripts, game logic, art, tests, or build instructions on its visible `main` tree.
- **Published prototype:** None identified for Mini Terra in the evidence reviewed.

This is *not* confirmation that today's Project Terra is an adventure/puzzle game. It is simply the most relevant documented candidate.

### 3.2 Current implemented functionality

**None verifiable for `mini-terra`.** No Unity project manifest, playable scene, input script, gameplay implementation, or tests are committed in the inspected branch. **No build or test has been executed.** This may not reflect uncommitted local work or another repository.

### 3.3 Information not found

No authoritative source was found specifying protagonist/units, story, world, perspective, camera, controls, core mechanics, puzzle rules, challenge/failure states, enemies/NPCs, map shape, level count, art/audio direction, intended platform, target engine version, saves, procedural systems, multiplayer, UI, or actual MVP acceptance criteria.

**Do not infer** exploration, environmental restoration, terraforming, planet management, RPG systems, building/crafting, combat, platforming, or any other mechanic from the name “Terra.”

## 4. Candidate vision statement — HOLD FOR USER CONFIRMATION

> **Only if** Mini Terra and Project Terra are the same idea: *Project Terra is a Unity-based adventure/puzzle game in which the player explores [TBD setting] and uses [TBD central mechanic] to overcome [TBD obstacles] and complete [TBD objective].*

Every bracket must be resolved, and the identity condition confirmed, before this is promoted to the actual pitch.

## 5. Implementation-grade build bible outline

The eventual **v1.0 handoff** must contain all the following with stable IDs, priority, deterministic behavior where relevant, source rationale, and acceptance tests:

1. **Game identity:** elevator pitch, genre, player fantasy, influences, non-goals, one differentiating mechanic.
2. **MVP boundary:** exactly what is shipped in the first playable, what is deferred, what counts as complete.
3. **Target environment:** engine and version, supported OS/input devices, screen orientation/aspect ratio, build target, performance goals.
4. **Player experience:** launch flow, tutorial/onboarding, moment-to-moment loop, mission/level loop, end-of-session flow, restart.
5. **World/camera/movement:** dimension, coordinate conventions, camera, locomotion, collisions, interactables, gravity or navigation as appropriate.
6. **Core gameplay systems:** inputs -> preconditions -> state transitions -> player-visible feedback -> failures and edge cases.
7. **Puzzles/challenges:** types, formal win/fail conditions, solution paths, reset behavior, hints, accessibility.
8. **Entities:** protagonist, NPCs, hazards, interactables, placement rules, state and persistence.
9. **Levels/content:** level maps, object placements, trigger conditions, sequence/gates, required assets and example test level.
10. **Narrative/style/audio:** any required fiction, visual reference, UI hierarchy, effects, placeholder policy, licenses.
11. **System/UI architecture:** scene graph, modules, data/assets, event/command boundaries, save format, dependency policy.
12. **AI/autonomy if any:** behaviors, authored vs systemic, failure boundaries; omit if not part of MVP.
13. **Testing/acceptance:** deterministic unit tests, play-mode tests, golden playthroughs, performance and usability criteria.
14. **Agent workflow:** repository instructions, work tickets, feature branches, PR review, code/documentation expectations, human approval gates.
15. **Decision log:** all unresolved matters, known conflicts, proposed alternatives, dates and decisions.
16. **Milestone backlog:** dependency-ordered tickets with testable definitions of done.

## 6. Clarifying questions — prioritized interview

All questions are unanswered unless indicated as conditional/historical. The group in **A** blocks an accurate gameplay specification; **B** blocks a complete MVP scope or architecture; **C** improves a production-quality handoff.

### A. Foundational: answer these first

**A01 — Repository identity [BLOCKER].** Is **Project Terra** the same game as 2021's **Mini Terra** (`bborumoore/mini-terra`), an evolution of it, or a completely separate project? If separate, what is the correct repo URL/account/branch?

**A02 — Existing work [BLOCKER].** Is there actual prototype code, art, documentation, notes, video, or a local Unity folder not yet committed to GitHub? Where is the authoritative material?

**A03 — One-sentence pitch [BLOCKER].** Describe the game in one or two sentences: who/what does the player control, what do they do, and what makes that fun?

**A04 — Genre [BLOCKER].** Is it still an adventure/puzzle game, or has the design changed? Are there other genres it should combine or explicitly avoid?

**A05 — Perspective [BLOCKER].** Is the MVP 2D side-on, 2D top-down/isometric, 3D third-person, 3D first-person, or something else?

**A06 — Core verb/mechanic [BLOCKER].** What exact action or interaction should players repeat most often? Explain one concrete example of how it changes the world or advances them.

**A07 — Objective [BLOCKER].** What is the player's immediate objective and what is the final win condition for the first playable level/session?

**A08 — Example playthrough [BLOCKER].** Narrate a realistic first 3–5 minutes, from opening the game to solving one obstacle and reaching success/failure.

**A09 — Challenge mechanics [BLOCKER].** What makes that goal difficult? Describe at least two example obstacles/puzzles with their rules and intended solutions.

**A10 — Player failure and recovery [BLOCKER].** Can the player die, lose progress, become stuck, or make irreversible choices? How do retry, checkpoints, restart, and undo work?

**A11 — Target player and session [BLOCKER].** Who is this for (casual, puzzle fans, sandbox fans, etc.) and how long should an MVP level/play session last?

**A12 — MVP proof [BLOCKER].** What must a stranger be able to do in a downloaded build to say “this demonstrates Project Terra”? What should explicitly be *out of scope*?

### B. Systems and experience

**B01 — Platforms.** Windows desktop only initially? WebGL? Linux? Mobile? Keyboard/mouse vs controller?

**B02 — Engine.** Should we explicitly commit to Unity/C# again, and if so, which version? Is a different engine acceptable?

**B03 — Controls.** Exactly what inputs move/aim/interact/activate abilities/open inventory/pause? Should inputs be remappable?

**B04 — Movement rules.** Speed, jumping/climbing/flying, collision, physics, camera bounds, handling of multiple movement modes?

**B05 — World.** What is the setting? What is the smallest representative environment we should build?

**B06 — Level topology.** One handcrafted level, multiple stages, seamless world, procedural areas, or a hub?

**B07 — Puzzle model.** Are puzzle mechanics based on switches/doors, object manipulation, traversal, physics, dialogue, logic, resource management, or something else? Which are required?

**B08 — State changes.** Which actions persist after leaving a room, respawning, restarting a level, or relaunching the game?

**B09 — Progression.** New abilities, unlocked areas, score, collectibles, upgrades, narrative stages, or none?

**B10 — Inventory/resources.** Does the MVP require items, equipment, energy, currencies, timers, or crafting?

**B11 — NPCs/enemies.** Do non-player characters or adversaries exist? If yes, which exact behaviors must appear in the MVP?

**B12 — Difficulty/hints.** Should challenges be discoverable purely through play, guided with hints, or offered with difficulty settings?

**B13 — Save/load.** Save after checkpoints, between levels, on quit, or not at all for the MVP?

**B14 — Determinism/RNG.** Are procedural generation and random events part of the concept? What must be reproducible in automated tests?

**B15 — UI flow.** Required screens: main menu, pause, settings, level select, inventory, game over, victory, instructions?

**B16 — Visual identity.** Pixel art, vector, stylized 3D, realistic 3D, low-poly, hand-painted, or an existing reference? Provide links/screenshots if possible.

**B17 — Story/tone.** Is narrative central, incidental, or absent? What feeling should players leave with?

**B18 — Sound.** What music, ambient audio, UI feedback and mechanics feedback are must-have versus placeholders?

**B19 — Accessibility.** Are there required readability, color/contrast, motion, timing, or input accommodations?

**B20 — Online or local.** Single-player offline only for MVP? Any plans for cooperative play, leaderboards, or account systems (likely later)?

### C. Implementation and agent-handoff expectations

**C01 — Code inheritance.** If source exists, should we preserve and refactor it, or start from a clean project?

**C02 — Art pipeline.** Will you supply assets, approve AI-generated placeholders, use licensed asset packs, or ask the agent to create its own?

**C03 — Architecture preference.** Do you have any fixed engineering rules (e.g., deterministic pure-domain model, dependency injection) or should architecture be proposed after mechanics are frozen?

**C04 — GitHub workflow.** Which repository should house the build bible? Should agents create issues and pull requests for your approval, with no direct main-branch merges?

**C05 — Learning documentation.** Should every PR include explanatory comments, `why/how` notes, a walkthrough of the code, and explicit local reproduction/testing instructions?

**C06 — Build/test environment.** Who/what can launch Unity and run EditMode/PlayMode tests? Is a Windows runner or local machine available?

**C07 — Acceptance gates.** Must an MVP pass a scripted successful playthrough, failure/retry playthrough, compile checks, automated tests, and a human playtest before calling it done?

**C08 — Constraints.** Desired timeline, team/agent budget, dependencies/license restrictions, offline requirements, minimum PC hardware?

**C09 — Scope guardrails.** Which ideas should be preserved for later while **forbidden** from entering MVP tickets?

**C10 — Source decisions.** What should happen when your earlier brainstorming conflicts with new decisions? Recommended: latest explicit answer wins, recorded in a dated change log.

## 7. Proposed MVP template — NOT approved game design

This is an **illustrative shape** only *if* Project Terra is the historical adventure/puzzle concept:

- Launch into a start menu and play one representative scene.
- Control a protagonist with a fully documented input scheme.
- Encounter a visible obstacle, understand how the core mechanic works, and solve **two representative challenges** using it.
- Receive immediate understandable feedback for success, wrong moves, and resets.
- Reach an unambiguous end-of-level success state; allow replay.
- Package a downloadable desktop build with automated baseline tests and a documented manual playthrough.

**Explicitly not canonical:** One scene, two puzzles, single-player, desktop, and the protagonist concept. These are candidate scope-reduction assumptions to approve or replace, not recovered facts. A different genre may require an entirely different MVP shape.

### Example acceptance scenario template

> **Given** [known world/puzzle starting state], **when** the player [documented input/action], **then** [exact authoritative state change], **and** the UI [specific feedback].  
> **Failure case:** Given [invalid state], when [action], then [no forbidden state mutation] and [clear feedback].  
> **Recovery:** After [reset/retry], [documented entities/states] return to [specified defaults].

## 8. Suggested agent handoff & governance (process proposal)

Once A01–A12 are answered:

1. Freeze **bible v1.0** with stable mechanic IDs (`MECH-001`), entity IDs (`ENT-001`), level IDs (`LVL-001`), rules, examples and edge cases.
2. Translate each feature into one or more GitHub issues containing: source section, objective, scope, non-goals, dependency, acceptance criteria, and test plan.
3. Agents branch from approved baseline; changes via PRs with design rationale, commented code, architecture impact, test evidence and “How to explore this implementation” learning notes.
4. Require user approval for changes to the game rules, external services, source-of-truth decisions, and merges to `main`.
5. Track **proposed**, **implemented**, **tested**, **playtested**, **accepted** separately. Code in repo does not establish playability without Unity build verification.

### Build-readiness checklist (all must be checked before autonomous MVP implementation)

- [ ] Correct repo and existing asset/code inventory identified
- [ ] Game identity, core loop, viewpoint, player, challenge mechanics, win/failure rules frozen
- [ ] Target platform/engine/build environment frozen
- [ ] Example playthroughs and minimum content inventory written
- [ ] All mandatory gameplay rules expressible as testable Given/When/Then cases
- [ ] Every edge-case ambiguity either decided or explicitly deferred out of scope
- [ ] MVP scope/exclusions approved
- [ ] Source art/audio rights and placeholder policy recorded
- [ ] Code/docs/testing/PR requirements recorded
- [ ] A human can run and verify the exact MVP acceptance script

## 9. Next interview sequence

**First: A01.** Are **Project Terra** and [`mini-terra`](https://github.com/bborumoore/mini-terra) the same game? If not, provide the correct repository or tell us no source exists.  
**Then: A03–A09.** Capture the fantasy, core action and a concrete playthrough.  
**Then: B01–B08.** Freeze the technical experience and systems.  
**Finally: A12 and C01–C08.** Freeze acceptance criteria, production workflow and MVP boundaries.

**No GitHub issues, branches, repositories, or files have been modified by this audit.**
