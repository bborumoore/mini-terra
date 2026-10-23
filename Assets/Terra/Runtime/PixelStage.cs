using System;
using System.Collections.Generic;
using Terra.Core;
using UnityEngine;

namespace Terra.Runtime
{
    /// <summary>Original, code-drawn placeholder sprites. Point filtering keeps the tiny textures crisp.
    /// All presentation reads the simulation; colors, text and camera never decide combat outcomes.</summary>
    public sealed class PixelStage : IDisposable
    {
        private readonly Transform root;
        private readonly List<UnityEngine.Object> generated = new List<UnityEngine.Object>();
        private readonly Sprite block;
        private readonly Camera camera;
        private readonly SpriteRenderer hero, boss, blade, warning, wave, seed, gate, bossAura;
        private int previousBossHealth = CombatTuning.MaxBossHealth;
        private float bossFlash;

        public PixelStage(Transform parent)
        {
            root = new GameObject("Pixel stage").transform;
            root.SetParent(parent);
            block = SpriteFrom(new[] { "W" }, new Dictionary<char, Color> { ['W'] = Color.white }, 1);
            var cameraObject = new GameObject("Terra Camera");
            cameraObject.transform.SetParent(root);
            camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 8f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Hex(0x141421);
            camera.transform.position = new Vector3(-10, 5.2f, -10);
            camera.nearClipPlane = .1f;
            camera.farClipPlane = 100f;
            cameraObject.tag = "MainCamera";

            // Village and arena share one coordinate space. There is no loading transition.
            Rect("Distant ridge", 0, 2, 75, 5, Hex(0x232332), -20);
            for (int i = -25; i < 30; i += 3)
            {
                Rect("Pillar silhouette", i, 4f, .7f, 7f + (i % 3), Hex(0x292936), -19);
                Rect("Ground slab", i, -.65f, 2.9f, 1.3f, Hex(0x36343c), -1);
                Rect("Ground edge", i, -.04f, 2.9f, .16f, Hex(0x68615c), 0);
            }
            House(-17f, 3f); House(-9f, 2.5f); House(-4f, 2f);
            Rect("Village shrine", -12f, .8f, .7f, 1.6f, Hex(0x797272), -5);
            Rect("Shrine light", -12f, 1.8f, .3f, .4f, Hex(0x8fd1bb), -4);
            Rect("Arena arch left", .2f, 3, .55f, 6, Hex(0x706652), -5);
            Rect("Arena arch right", 25.5f, 3, .55f, 6, Hex(0x706652), -5);
            for (int i = 4; i < 26; i += 5)
            {
                Rect("Broken arena column", i, 2f, .9f, 4f, Hex(0x47434a), -10);
                Rect("Column crown", i, 4f, 1.4f, .3f, Hex(0x59535a), -9);
            }
            hero = Art("Hero", new[] {
                "....HHHH....", "...HFFFFH...", "...HFEEF....", "....FFFF....", "...RRRRRR...",
                "..RRBRBBRR..", "..RRBBBBRR..", "...BBBBBB...", "...BBBBBB...", "...BB..BB...",
                "...BB..BB...", "..DDD..DDD.."
            }, new Dictionary<char, Color> { ['H'] = Hex(0xaaa6b8), ['F'] = Hex(0xc6aa9d), ['E'] = Hex(0x34303d), ['R'] = Hex(0xa03f57), ['B'] = Hex(0x555b73), ['D'] = Hex(0x242532) }, 8, 10);
            boss = Art("Cairn Warden", new[] {
                "......SSSSSS......", ".....SSSSSSSS.....", "....SSSEEEESSS....", "....SSSE..ESSS....",
                "....SSSSSSSSSS....", "..SSSSSSSSSSSSSS..", ".SSSSSCCCCCCSSSSS.", ".SSSSSCCCCCCSSSSS.",
                ".SSSSSCCLLCCSSSSS.", ".SSSSSCCLLCCSSSSS.", "..SSSSCCCCCCSSSS..", "...SSSSSSSSSSSS...",
                "....SSSSSSSSSS....", "....SSSS..SSSS....", "....SSSS..SSSS....", "...SSSSS..SSSSS..."
            }, new Dictionary<char, Color> { ['S'] = Hex(0x8d8065), ['C'] = Hex(0x514d49), ['E'] = Hex(0xeac777), ['L'] = Hex(0xa6c59d) }, 5, 8);
            blade = Rect("Sword active arc", 0, 1, CombatTuning.SwordReach, .45f, Hex(0xe9dfba), 12);
            warning = Rect("Faultline marker", 0, .1f, 4.5f, .2f, Hex(0xecad5c), 5);
            wave = Rect("Travelling quake", 0, .45f, 1.1f, .9f, Hex(0xf4c06b), 7);
            seed = Rect("Earth seed", CombatTuning.BossX, .8f, .4f, .6f, Hex(0xb5dc9b), 11);
            gate = Rect("Arena seal", .2f, 2f, .2f, 4f, Hex(0xcf785f), 5);
            bossAura = Rect("Quake warning", CombatTuning.BossX, .1f, 4, .18f, Hex(0xecad5c), 5);
        }

        public void Render(CombatSession s)
        {
            camera.transform.position = new Vector3(s.InArena ? 12.5f : -10f, 5.2f, -10f);
            // Pixel snap only the displayed sprite; the authoritative motion stays subpixel precise.
            hero.transform.position = new Vector3(Snap(s.PlayerX), Snap(s.PlayerY), 0);
            hero.flipX = s.Facing < 0;
            hero.color = s.HurtRemaining > 0f ? new Color(1, .65f, .65f, .65f) : Color.white;
            boss.transform.position = new Vector3(CombatTuning.BossX, 0, 0);
            boss.enabled = s.BossHealth > 0;
            if (s.BossHealth < previousBossHealth) bossFlash = .12f;
            if (!s.Paused) bossFlash = Mathf.Max(0, bossFlash - Time.deltaTime);
            previousBossHealth = s.BossHealth;
            boss.color = bossFlash > 0 ? new Color(1, .6f, .5f) : s.Phase == BossPhase.Telegraph ? new Color(1, .85f, .6f) : Color.white;
            blade.enabled = s.SwordActive;
            blade.transform.position = new Vector3(s.PlayerX + s.AttackDirection * 1.3f, s.PlayerY + 1f, 0);
            bool live = s.State == EncounterState.Fighting;
            warning.enabled = live && s.Move == BossMove.Faultline && s.Phase != BossPhase.Recovery;
            warning.transform.position = new Vector3(s.TargetX, s.Phase == BossPhase.Active ? .75f : .1f, 0);
            warning.transform.localScale = new Vector3(4.5f, s.Phase == BossPhase.Active ? 1.5f : .2f, 1);
            warning.color = s.Phase == BossPhase.Active ? Hex(0xe6885f) : Hex(0xecad5c);
            bossAura.enabled = live && s.Move == BossMove.Quake && s.Phase == BossPhase.Telegraph;
            wave.enabled = live && s.WaveActive;
            wave.transform.position = new Vector3(s.WaveX, .45f, 0);
            seed.enabled = s.SeedAvailable;
            gate.enabled = live;
        }

        private void House(float x, float height)
        {
            Rect("Abandoned house", x, height / 2, 3, height, Hex(0x3a3542), -8);
            Rect("Roof", x, height, 3.7f, .55f, Hex(0x62505a), -7);
            Rect("Door", x, .7f, .7f, 1.4f, Hex(0x1e1c29), -6);
            Rect("Window", x + .85f, 1.5f, .35f, .45f, Hex(0x89715b), -6);
        }
        private SpriteRenderer Rect(string name, float x, float y, float width, float height, Color color, int order)
        {
            var renderer = Renderer(name, block, order);
            renderer.transform.position = new Vector3(x, y, 0);
            renderer.transform.localScale = new Vector3(width, height, 1);
            renderer.color = color;
            return renderer;
        }
        private SpriteRenderer Art(string name, string[] pixels, Dictionary<char, Color> palette, float ppu, int order)
            => Renderer(name, SpriteFrom(pixels, palette, ppu, true), order);
        private SpriteRenderer Renderer(string name, Sprite sprite, int order)
        {
            var go = new GameObject(name);
            go.transform.SetParent(root);
            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = order;
            return renderer;
        }
        private Sprite SpriteFrom(string[] rows, Dictionary<char, Color> palette, float ppu, bool feetPivot = false)
        {
            int width = rows[0].Length, height = rows.Length;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    texture.SetPixel(x, height - 1 - y, palette.TryGetValue(rows[y][x], out Color c) ? c : Color.clear);
            texture.Apply();
            var sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(.5f, feetPivot ? 0 : .5f), ppu);
            generated.Add(sprite); generated.Add(texture);
            return sprite;
        }
        private static float Snap(float value) => Mathf.Round(value * 32f) / 32f;
        private static Color Hex(int value) => new Color(((value >> 16) & 255) / 255f, ((value >> 8) & 255) / 255f, (value & 255) / 255f);
        public void Dispose()
        {
            foreach (var asset in generated) UnityEngine.Object.Destroy(asset);
            if (root != null) UnityEngine.Object.Destroy(root.gameObject);
        }
    }
}
