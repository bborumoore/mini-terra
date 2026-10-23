#!/usr/bin/env python3
"""Run unchanged C# specifications when the dotnet CLI cannot inspect /proc.
Normal environments should use: dotnet run --project Tests/Headless/Terra.Specs.csproj
Usage: python3 tools/run_specs_direct.py /absolute/path/to/dotnet-sdk-root
"""
import json
from pathlib import Path
import subprocess
import sys
import tempfile

sdk = Path(sys.argv[1]).resolve()
root = Path(__file__).resolve().parents[1]
compiler = sorted(sdk.glob('sdk/8.*/Roslyn/bincore/csc.dll'))[-1]
reference = sorted(sdk.glob('packs/Microsoft.NETCore.App.Ref/8.*/ref/net8.0'))[-1]
with tempfile.TemporaryDirectory(prefix='terra-specs-') as temporary:
    output = Path(temporary)
    assembly = output / 'Terra.Specs.dll'
    args = ['/nologo', '/target:exe', '/langversion:9', '/out:' + str(assembly)]
    args += ['/reference:' + str(path) for path in reference.glob('*.dll')]
    args += [str(root / path) for path in (
        'Assets/Terra/Core/CombatSession.cs',
        'Assets/Terra/Tests/EditMode/CombatSpecifications.cs',
        'Tests/Headless/Program.cs')]
    subprocess.run([str(sdk / 'dotnet'), str(compiler), *args], check=True)
    (output / 'Terra.Specs.runtimeconfig.json').write_text(json.dumps({
        'runtimeOptions': {'tfm': 'net8.0', 'framework': {
            'name': 'Microsoft.NETCore.App', 'version': '8.0.0'}}}))
    sys.exit(subprocess.run([str(sdk / 'dotnet'), str(assembly)]).returncode)
