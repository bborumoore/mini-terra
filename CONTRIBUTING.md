# Contributing to Project Terra

## Workflow
1. Create or select an issue with source rule IDs, scope, acceptance criteria, dependencies, and verification steps. Resolve design blockers with Brian before implementing mechanics.
2. Branch from current main: `feat/<issue>-<description>`, `fix/<issue>-<description>`, or `chore/<description>`.
3. Keep changes focused. Update documentation with behavior changes. Explain non-obvious invariants and tradeoffs in code comments; include worked examples for gameplay rules.
4. Run relevant checks. Record exact commands, editor version, commit, results and limitations. Never describe unexecuted tests as passing.
5. Open a PR linked to the issue. Explain why/how, architecture impact, testing, risks, and a short learning walkthrough.
6. Brian reviews the result and game-rule changes. Merge only after review and required checks; do not push directly to main. Squash merge is recommended for focused feature PRs.
7. Close an issue only when its acceptance criteria are satisfied. Use `Closes #N` only for fully resolved work; otherwise use `Refs #N`. Keep build/playtest blockers open.

## Unity source control
Commit Assets and their .meta files together, Packages manifests/lockfiles and ProjectSettings. Never regenerate existing GUIDs gratuitously. Exclude Library, Temp, Logs, builds, generated IDE files, secrets and license credentials. Once the editor is selected, use visible meta files and Force Text serialization. Document editor upgrades explicitly.

## Validation and status
Track proposed, implemented, tested, playtested, and accepted separately. Runtime code needs Unity compile and relevant EditMode/PlayMode evidence. A downloadable build needs a launch check and the approved manual playthrough. Documentation-only PRs need source/link review, not artificial runtime tests.

## Repository administration
Branch protection and mandatory checks are recommended once actual CI checks exist. These instructions document policy; they do not configure GitHub branch protection. Do not invent successful CI or add unusable license-dependent gates.
