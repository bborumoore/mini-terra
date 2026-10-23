using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Terra.Editor
{
    public static class BuildPlayer
    {
        [MenuItem("Terra/Build Windows prototype")]
        public static void Windows()
        {
            const string scene = "Assets/Terra/Scenes/FirstCombat.unity";
            const string output = "Builds/Windows/ProjectTerra.exe";
            Directory.CreateDirectory("Builds/Windows");
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { scene },
                locationPathName = output,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("Windows build failed: " + report.summary.result);
        }
    }
}
