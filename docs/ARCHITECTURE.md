# First combat architecture

## Ownership and data flow

`TerraBootstrap` creates input actions, a `CombatSession`, and a `PixelStage`. The committed `FirstCombat` scene contains that bootstrap component. Runtime-created presentation keeps the placeholder scene reproducible without importing third-party art.

Update samples device input and latches button edges. FixedUpdate consumes them in 1/120-second simulation steps. LateUpdate reads the result and displays it. The HUD reads the same state, so it cannot independently award damage or seed pickups.

`Terra.Core` has no Unity dependencies. `Terra.Runtime` references Core and Unity Input System. `Terra.Editor` contains only the build entry point. Test assemblies remain outside player builds. This is a small prototype architecture; do not introduce a general ECS, service locator or ability framework before actual second/third mechanics justify one.

## Worked sword hit

1. A mouse/J press becomes `CombatInput.AttackPressed`.
2. If not already swinging or dashing, the simulation starts a 0.30-second swing and remembers its direction.
3. At 0.06–0.16 seconds, the sword checks horizontal distance, facing, and vertical overlap.
4. On a hit it subtracts 20 HP and marks this swing as having hit. Repeated active ticks cannot deal extra damage.
5. A lethal hit switches the encounter to Victory, cancels the boss hazard, and exposes one collectible seed.
6. PixelStage displays the sword and boss flash from this authoritative result.

Player attacks precede boss damage within a tick. That simultaneous-hit priority is provisional and documented in the bible, not an accidental promise of the campaign rules.

## Movement tradeoff

The first encounter has a flat floor and fixed boundaries. Its motor uses analytical positions, gravity and a floor clamp, not Unity colliders. This makes precise first-fight tuning easy to test. It intentionally does not support arbitrary Tilemap terrain, slopes or platforms. Add a collision/motor abstraction and corresponding terrain tests before expanding traversal. Rendering snaps positions to a 1/32-unit grid without quantizing authoritative movement.

There is no coyote-time system because there are no ledges. There is a 120 ms pre-landing jump buffer, variable jump height and immediate horizontal response. Input bindings are outside Core so controller support can be added without rewriting combat rules. Gamepad bindings/remapping UI are not yet implemented.

## Boss fairness and limitations

Faultline locks its target at telegraph start. Quake locks its direction. Neither secretly tracks during the warning. Both are visually labelled and use different shapes; color is not the only cue. Damage grace prevents one persistent hitbox from dealing damage on every tick. The stationary two-move boss is a first design proposal and needs human balancing; it is intentionally not called a release-quality encounter.

## Lifecycle

Pause freezes simulation and clears queued input. Focus loss pauses. Death disables the attempt; R resets all prototype state. Victory clears hazards and unlocks the arena exit. The seed remains available until a nearby E press; repeated presses cannot duplicate it. R after victory is explicitly a new practice attempt, not the campaign's three-seed world transition.

No disk persistence or full world-seed bank exists yet. Choosing persistence before deciding what survives death would harden an unresolved design rule.

## Testing and environment

`CombatSpecifications.cs` contains command-driven scenarios shared by the .NET runner and Unity EditMode adapter. The headless runner uses reflection only to discover the specification methods; runtime gameplay does not use reflection. The PlayMode smoke test exercises the committed scene, camera and objects but does not replace a controls/feel playtest.

The development workspace has no Unity editor. .NET SDK installation extracted its files but reported unsupported ownership changes; the CLI also cannot read process start time in this environment. The included `tools/run_specs_direct.py` invokes the SDK's Roslyn compiler and .NET runtime directly, executing the same production Core and specification sources. On a normal workstation/CI use the simpler `dotnet run` command.

## Dependency references

- Unity release: https://unity.com/releases/editor/whats-new/6000.5.9f1
- Input System 1.17 documentation: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/index.html
- InputActionMap ownership/disposal: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/api/UnityEngine.InputSystem.InputActionMap.html

Package versions are pinned in the manifest; the resolved lockfile and generated defaults require a real Unity import. No Unity validation claim is inferred from the .NET tests.
