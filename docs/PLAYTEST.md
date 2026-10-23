# First-combat playtest

Use Unity 6000.5.9f1 and the exact feature-branch commit. Record CPU/GPU, window size, editor versus Windows build, date, logs, and results. This checklist is intentionally uncompleted until an actual Unity session is run.

- [ ] Clean import: no Console compile errors; Package Manager resolves; commit the generated lockfile.
- [ ] Open FirstCombat and press Play. Village, hero, HUD, floor and instructions are visible. No missing/magenta graphics.
- [ ] Tap A/D, stop, and reverse. Response should feel immediate with no unexpected sliding.
- [ ] Compare short Space tap and held jump. Press shortly before landing and verify buffered jump.
- [ ] Attack with mouse and J. Dash with Shift and RMB. Hold/repeat inputs; cooldowns and one-hit-per-swing rules remain consistent.
- [ ] Walk east. Camera switches to arena framing; player remains visible and cannot exit during the active fight.
- [ ] Observe Faultline: the marked patch stays fixed after warning. Step away/jump; damage happens only when it erupts.
- [ ] Observe Quake: jump the travelling wave. Watch recovery and attack during it.
- [ ] Stand in hazards until death. Controls cannot continue the fight. R returns to a full-health, fresh village attempt.
- [ ] Win the fight. Boss and hazards disappear; approaching the seed and pressing E collects it once. Press E repeatedly.
- [ ] Walk west to the village. Seed count remains 1/3. The prototype does not claim a completed run.
- [ ] R starts a fresh practice attempt after victory, clearing that demonstration seed.
- [ ] Pause mid-warning and mid-jump for several seconds; resume at the same gameplay state. Menu clicks do not become attacks.
- [ ] Alt-tab away and back: paused until explicitly resumed.
- [ ] Resize the window / use a different aspect ratio. Instructions, health and combat remain readable.
- [ ] Run EditMode specifications and PlayMode scene smoke test; attach actual output.
- [ ] Build with Terra → Build Windows prototype. Launch outside Unity and repeat a win and a death/retry.

## Feedback that will guide iteration

Does movement feel precise? Is the jump too high/low or floaty? Does dash help, and should it eventually have invulnerability? Can you read each attack without instructions? Is the recovery long enough? Does jumping and attacking repeatedly trivialize the encounter? What would make this boss more distinctive?

## Acceptance status

Unity import: NOT RUN here. Unity tests: NOT RUN here. Windows build: NOT RUN here. Human feel/balance: NOT RUN. Pure C# logic is tested separately; see VALIDATION.md.
