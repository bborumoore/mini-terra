# Prototype validation — 2026-10-22

## Executed in the development workspace

- Roslyn C# compiler from .NET SDK 8.0.426 compiled the real `CombatSession.cs`, shared `CombatSpecifications.cs`, and console runner with C# 9 language rules.
- 18/18 gameplay specifications passed, including input-only victory and death, jump height/buffering, movement stopping, locked boss aim, one-hit-per-swing, facing/range, damage grace, pause, dash cooldown, proximity/duplicate seed collection, reset and replay determinism.
- Static project validation checks meta GUID uniqueness, scene/build references, JSON manifests, pixel sprite row sizes and local documentation links.

Reproduce this workspace's direct compiler run with:

```sh
python3 tools/run_specs_direct.py /tmp/terra-dotnet
python3 tools/validate_project.py
```

On a normal workstation or GitHub Actions, use `dotnet run --project Tests/Headless/Terra.Specs.csproj`. That CLI command could not run in this workspace because its process-start-time query fails; invoking its compiler and runtime directly succeeded. This does not alter the compiled production/test source.

## Not executed here

Unity is unavailable. No claim is made for Unity compilation, package resolution/import, EditMode/PlayMode execution, rendering, input-device integration, Windows packaging, performance or human playability. A committed scene smoke test and build entry point are provided for those next checks. The GitHub Actions workflow runs only headless specifications and static packaging validation, not Unity.

The gameplay win test is deliberately simple and reveals a balance limitation: repeated jumps/attacks can defeat this stationary prototype. Human feedback should guide the next encounter iteration before more bosses are added.
