#!/usr/bin/env python3
"""Static packaging checks only. Does not assert Unity compiles or renders the game."""
import json
from pathlib import Path
import re

root = Path(__file__).resolve().parents[1]
errors = []
guids = {}
for asset in (root / 'Assets').rglob('*'):
    if asset.suffix == '.meta':
        if not Path(str(asset)[:-5]).exists(): errors.append(f'Orphan meta: {asset.relative_to(root)}')
        continue
    meta = Path(str(asset) + '.meta')
    if not meta.exists():
        errors.append(f'Missing meta: {asset.relative_to(root)}')
        continue
    found = re.search(r'^guid: ([a-f0-9]{32})$', meta.read_text(), re.M)
    if not found: errors.append(f'Malformed GUID: {meta.relative_to(root)}'); continue
    guid = found.group(1)
    if guid in guids: errors.append(f'Duplicate GUID: {guid}')
    guids[guid] = asset
    if asset.suffix == '.asmdef': json.loads(asset.read_text())
for path in [root / 'Assets/Terra/Scenes/FirstCombat.unity', root / 'ProjectSettings/EditorBuildSettings.asset']:
    for guid in re.findall(r'guid: ([a-f0-9]{32})', path.read_text()):
        if guid not in guids: errors.append(f'Unresolved scene/build GUID: {guid}')
manifest = json.loads((root / 'Packages/manifest.json').read_text())
assert manifest['dependencies']['com.unity.inputsystem'] == '1.17.0'
assert '6000.5.9f1' in (root / 'ProjectSettings/ProjectVersion.txt').read_text()
# A row-length typo in the code-authored sprite would throw during scene startup.
stage = (root / 'Assets/Terra/Runtime/PixelStage.cs').read_text()
for block in re.findall(r'Art\([^\n]+new\[\] \{(.*?)\}', stage, re.S):
    rows = re.findall(r'"([.A-Z]+)"', block)
    if len({len(row) for row in rows}) != 1: errors.append('Inconsistent pixel sprite row widths')
for doc in [root / 'README.md', root / 'BUILD_BIBLE.md', *(root / 'docs').glob('*.md')]:
    for target in re.findall(r'\]\(([^)]+)\)', doc.read_text()):
        if '://' not in target and not target.startswith('#'):
            if not (doc.parent / target.split('#')[0]).exists(): errors.append(f'Broken local link in {doc.name}: {target}')
if errors:
    raise SystemExit('\n'.join(errors))
print(f'PASS: {len(guids)} stable asset GUIDs; scene/build references, manifests, sprite rows and local doc links valid.')
print('Unity import/compile/rendering are NOT checked by this script.')
