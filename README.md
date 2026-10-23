# Project Terra

A Unity 2D combat game about defeating elemental bosses and reshaping a bleak world. The [living build bible](BUILD_BIBLE.md) records confirmed design and provisional combat tuning separately.

## Current prototype

Village → Earth boss → seed pickup → return to village. Walk/jump, sword attack, one dash skill, readable attack warnings, health, pause and fresh-attempt reset. Original pixel placeholders. The full three-boss/world-seed campaign is not implemented.

**Status:** gameplay specifications pass outside Unity; Unity import, scene tests, Windows build and human playtest are still required. This is reviewable prototype source, not a verified downloadable game.

## Open and play

1. Clone this repository and check out `feat/6-first-combat` (until its PR is merged).
2. In Unity Hub, add the repository folder as a project and select **6000.5.9f1**. Install Windows Build Support if needed.
3. Allow Package Manager to resolve the pinned manifest. Commit the generated `Packages/packages-lock.json` after a successful first import; no lockfile is fabricated here.
4. Open `Assets/Terra/Scenes/FirstCombat.unity` and press Play. Focus the Game view to receive input.
5. Walk right from the village to enter the arena. The boss alternates a marked ground eruption and a travelling wave. Defeat it, approach the seed and press E. Walk left to return.

| Action | Binding |
| --- | --- |
| Move | A/D or Left/Right arrows |
| Jump | Space; release early for a short hop |
| Sword | Left mouse or J |
| Dash | Left Shift or right mouse; no invulnerability |
| Collect seed | E nearby |
| Pause/resume | Escape; losing focus also pauses |
| Fresh practice attempt | R after victory/death |

## Build Windows

Use **Terra → Build Windows prototype**. Output: `Builds/Windows/ProjectTerra.exe`. Share the entire Windows output folder, not the executable alone.

PowerShell from the repository root (with Unity closed):

```powershell
$unityEditor = 'C:\Program Files\Unity\Hub\Editor\6000.5.9f1\Editor\Unity.exe'
& $unityEditor -batchmode -quit -projectPath "$PWD" -executeMethod Terra.Editor.BuildPlayer.Windows -logFile build-windows.log
```

This requires a licensed/activated local Unity installation and the Windows build module. No credentials belong in this repository.

## Verification

Pure gameplay specifications, using .NET SDK 8:

```sh
dotnet run --project Tests/Headless/Terra.Specs.csproj
python3 tools/validate_project.py
```

In Unity: **Window → General → Test Runner**, then run EditMode and PlayMode. The latter loads the actual committed scene and checks its composition. Run the [manual playtest](docs/PLAYTEST.md) next. .NET tests cannot verify Unity APIs, package resolution, input handling, rendering or game feel.

## Learn the implementation

Start with [architecture and a worked attack](docs/ARCHITECTURE.md), [contributing](CONTRIBUTING.md), and [current decisions/backlog](docs/PROJECT_STATUS.md). All numeric combat settings are provisional and centralized in `CombatTuning` or documented boss phases. The original [discovery dossier](docs/reference/PROJECT_TERRA_BUILD_BIBLE_DISCOVERY_V0_1.md) is retained as history.
