# Project Terra — Living Build Bible

Version 0.2 · 2026-10-22 · Owner/game director: Brian Moore

This is the current design record. It supersedes the unconfirmed adventure/puzzle premise in the preserved [discovery dossier](docs/reference/PROJECT_TERRA_BUILD_BIBLE_DISCOVERY_V0_1.md). Latest explicit user decisions take precedence. **Confirmed** means Brian specified it; **prototype proposal** means a reversible implementation choice awaiting playtest. Neither source code nor a passing unit test proves the game feels good.

## 1. Vision — confirmed

You are a largely solitary hero trying to reshape a bleak world. Project Terra is a precise, difficult, fair 2D side-view combat game, structurally close to a boss rush. The emotional high is defeating unique bosses; the secondary reward is seeing the scenery change as the world seed develops. There are no puzzles and minimal exploration.

Mechanical/style references: Rogue Legacy and Momodora: Moonlit Farewell. Pixel art uses a dark palette, with early Dragon Warrior entries as an additional art reference. These are references, not assets to copy.

## 2. Production decisions — confirmed

| ID | Decision |
| --- | --- |
| TECH-001 | Fresh implementation in bborumoore/mini-terra; do not inherit unsuccessful local work. |
| TECH-002 | Unity 6000.5.9f1, C#, 2D side view. |
| TECH-003 | Windows and keyboard/mouse first. Plan controller support and Mac/Linux later. |
| TECH-004 | Issues → feature branches → educational pull requests. Brian reviews changes. |
| SCOPE-001 | First playable: village scene and one boss fight. Develop bosses individually. |
| STYLE-001 | Dark pixel art; bleak, mostly solitary atmosphere. |

## 3. Player and combat — confirmed

- COMBAT-001: Walking, jumping, attacking, and one or perhaps two equipped abilities. Exact ability count remains open.
- COMBAT-002: Movement must be precise and reward skill. Snappy, responsive combat is the primary quality target.
- EQUIP-001: Player can acquire/swap weapons, abilities and movement skills. Exact inventory and swap restrictions are open.
- MOVE-001: Traversal through earth, water and air requires unlocking the respective movement abilities; exact behaviors and unlock persistence are open.
- NPC-001: No NPCs or combat companions in current scope. Earlier mention of dialogue is superseded by the later clarification that NPCs are not currently planned.
- UPGRADE-001: An upgrade system is undecided.

## 4. World and run structure — confirmed

- WORLD-001: Continuous world. Village is the central screen. Every other active encounter screen contains a unique, balanced boss fight.
- WORLD-002: Earth, wind, water and fire each have tier 1/2/3 bosses along cardinal directions. Exact direction assignment is open.
- WORLD-003: Diagonals contain mixed-element encounters based on the direction taken. Coordinates, tier assignment and reward rules are open.
- RUN-001: Each run begins in the village. The player selects starting equipment/abilities and can obtain hints about the current world.
- RUN-002: Killing a boss awards a seed of that boss's element.
- RUN-003: Collecting the third seed ends the run and starts a new world. The collected elements carry forward in the world seed.
- SEED-001: The world seed grants bonuses to the player and bosses. Exact bonuses are unspecified.
- SEED-002: Initial-world bosses have only their native element. In subsequent worlds they gain the world seed's elements. Elemental variants must be distinct encounters, not merely palette/stat swaps.
- SEED-003: Once three of an element are stored, its mana is depleted; associated bosses stop spawning or remain weakened/non-interactive. Which presentation and mixed-boss behavior apply is open.
- END-001: Starting a world with three of every element in the world seed spawns the ultimate boss. Defeating it is final success for the initial release.
- DEATH-001: Death ends the run and erases all world-seed progress.
- VILLAGE-001: Village has buildings that can improve over the game. What survives death and how improvements work are unresolved.

### Worked campaign example — conditional interpretation

If every boss drops exactly one elemental unit and each third pickup banks the three collected units, four successful runs can yield Earth 3 / Wind 3 / Water 3 / Fire 3. The next world then qualifies for the ultimate boss. This illustrates the stated target; it does not settle mixed-element drops, duplicate caps, collection timing, or whether a boss kill auto-collects a seed.

### Campaign questions still requiring decisions

1. Do village upgrades, unlocked gear and movement abilities survive death, or does death reset those too?
2. Does a mixed boss drop one chosen element, a fixed element, multiple elemental units, or one mixed seed? What counts toward the three-seed run limit?
3. Are seeds picked up manually after a kill or awarded automatically? Can a player leave one behind?
4. Are the current run's elements applied immediately or only when a new world begins? Does order matter or only counts?
5. When an element reaches three, do mixed bosses containing it also disappear? How are remaining paths kept traversable?
6. Which element maps to each direction, and how does a side-view character reach north/south and diagonal screens before traversal unlocks?
7. Can players revisit the village, change equipment or heal mid-run? Can they escape an active boss?
8. Where does the ultimate boss appear, and what happens to ordinary bosses in that world?
9. What is retained when quitting and reopening the game? Saving/exiting is distinct from death.

## 5. First combat prototype — implementation proposals

The implementation deliberately samples combat without implementing campaign rules that remain ambiguous. These choices are exposed in code/docs for Brian to change after playtesting.

| ID | Proposed behavior |
| --- | --- |
| PROTO-001 | Adjacent village and Earth arena in one scene; walk east to enter, west to return after victory. |
| PROTO-002 | One sword; one dash skill. No equipment UI or unlock economy yet. |
| PROTO-003 | Immediate horizontal response at 8 units/s; full jump launch 15 units/s; gravity 40 units/s²; release cuts ascent; 120 Hz simulation. |
| PROTO-004 | 120 ms jump/attack input buffers. Flat ground only; ledges, slopes and coyote time are deferred to terrain work. |
| PROTO-005 | Sword: 20 damage, 0.30 s swing, active at 0.06–0.16 s, one hit per swing. Facing commits at startup; movement stays available. |
| PROTO-006 | Dash: 19 units/s for 0.14 s, 0.8 s cooldown, no invulnerability, no startup during a sword swing. No stamina/parry/block system yet. |
| PROTO-007 | Player 100 HP; boss 180 HP; boss hits deal 25 with 0.7 s damage grace. No contact damage or healing. |
| PROTO-008 | Cairn Warden (working name) alternates locked-position Faultline and a ground Quake wave. Warnings precede damage and recovery offers an opening. |
| PROTO-009 | Arena locks during the encounter. Lethal sword hit resolves before boss damage in the same tick; death/victory clear hazards. |
| PROTO-010 | Victory drops a manually collected Earth seed. HUD shows 1/3, but no fake three-boss completion occurs. R explicitly starts a new practice attempt. |
| PROTO-011 | Escape pauses. Loss of window focus pauses. Queued menu input is discarded on resume. |
| PROTO-012 | Original code-drawn pixel silhouettes and structures are temporary presentation; audio and final animation are not present. |

### Boss encounter timing

Opening recovery: 1.0 s. Then alternate:

| Move | Telegraph | Active | Recovery | Player response |
| --- | --- | --- | --- | --- |
| Faultline | 0.9 s; marked ground locks to player position (clamped within arena) | 0.22 s; 4.5-unit-wide ground eruption, height 1.5 | 1.1 s | Move out or jump over the eruption |
| Quake | 0.8 s; direction locks toward player | Up to 1.8 s; ground wave at 12 units/s, height 0.9 | 1.1 s | Jump the wave or reposition behind the boss |

The regular pattern is an intentional learning aid for the initial feel test. It is not yet a balanced final boss, and input-only tests show it can be defeated by repeated jumping/attacking. Human testing must determine whether that trivializes the fight.

## 6. First-playable acceptance

- Given a fresh launch, the village, player, controls and health are visible.
- Given movement input, the player responds immediately; stopping input stops horizontal movement.
- Given a held versus released jump, both land correctly but have distinct heights.
- Given a boss warning, aim/direction remains committed and damage begins only in the active phase.
- Given one sword swing overlapping the boss, it inflicts one hit only; range/facing matter.
- Given player death, input cannot continue the encounter and retry restores a clean village attempt.
- Given victory, hazards clear, the seed can be collected once at close range, and the village exit opens.
- Given pause, all gameplay freezes; resuming does not execute buffered menu actions.
- A Windows build from the exact reviewed commit launches and Brian completes the playtest in docs/PLAYTEST.md.

## 7. Implementation status

Gameplay source and scene are implemented on the feature branch. Pure C# specifications are executable without Unity. Unity import/compile, EditMode/PlayMode execution, Windows build and human playtest must be recorded separately. Campaign progression, variants, additional bosses, inventory, village improvements, traversal unlocks, saves, controller, Mac/Linux and audio remain unimplemented.

See [architecture](docs/ARCHITECTURE.md), [playtest](docs/PLAYTEST.md), and [project status](docs/PROJECT_STATUS.md).
