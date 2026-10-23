using Terra.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Runtime
{
    /// <summary>Composition root: input -> fixed simulation -> pixel presentation. No campaign save is written.</summary>
    public sealed class TerraBootstrap : MonoBehaviour
    {
        public CombatSession Session { get; private set; }
        private InputActionMap controls;
        private InputAction move, jump, attack, dash, interact, pause, retry;
        private CombatInput pending;
        private PixelStage stage;
        private GUIStyle title, text, small;

        private void Awake()
        {
            Session = new CombatSession();
            stage = new PixelStage(transform);
            controls = new InputActionMap("Combat");
            move = controls.AddAction("Move", InputActionType.Value);
            move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
            move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");
            jump = Button("Jump", "<Keyboard>/space");
            attack = Button("Attack", "<Mouse>/leftButton", "<Keyboard>/j");
            dash = Button("Skill", "<Keyboard>/leftShift", "<Mouse>/rightButton");
            interact = Button("Interact", "<Keyboard>/e");
            pause = Button("Pause", "<Keyboard>/escape");
            retry = Button("NewPracticeAttempt", "<Keyboard>/r");
            // Future gamepad bindings belong here or in an InputActionAsset; the simulation is device-agnostic.
            controls.Enable();
        }

        private InputAction Button(string name, params string[] bindings)
        {
            var action = controls.AddAction(name, InputActionType.Button);
            foreach (string binding in bindings) action.AddBinding(binding);
            return action;
        }

        private void Update()
        {
            if (pause.WasPressedThisFrame()) { TogglePause(); return; }
            if (Session.Paused) return;
            if (retry.WasPressedThisFrame() && (Session.State == EncounterState.Dead || Session.State == EncounterState.Victory))
            { NewAttempt(); return; }
            pending.Move = move.ReadValue<float>();
            // OR-latch press/release edges until a physics tick consumes them. At high render FPS,
            // several Updates can occur before FixedUpdate, so assigning would lose button presses.
            pending.JumpPressed |= jump.WasPressedThisFrame();
            pending.JumpReleased |= jump.WasReleasedThisFrame();
            pending.AttackPressed |= attack.WasPressedThisFrame();
            pending.DashPressed |= dash.WasPressedThisFrame();
            pending.InteractPressed |= interact.WasPressedThisFrame();
        }

        private void FixedUpdate()
        {
            float remaining = Time.fixedDeltaTime;
            var command = pending;
            while (remaining > .000001f)
            {
                float step = Mathf.Min(remaining, 1f / 120f);
                Session.Step(step, command);
                command = new CombatInput { Move = pending.Move };
                remaining -= step;
            }
            pending = new CombatInput { Move = pending.Move };
        }

        private void LateUpdate() { stage.Render(Session); }
        private void OnApplicationFocus(bool focused)
        {
            if (!focused && Session != null) { Session.SetPaused(true); pending = default; }
        }
        private void TogglePause() { Session.SetPaused(!Session.Paused); pending = default; }
        private void NewAttempt() { Session.Reset(); pending = default; }
        private void OnDestroy() { controls?.Dispose(); stage?.Dispose(); }

        private void OnGUI()
        {
            if (Session == null) return;
            if (title == null)
            {
                title = new GUIStyle(GUI.skin.label) { fontSize = 26, fontStyle = FontStyle.Bold };
                text = new GUIStyle(GUI.skin.label) { fontSize = 18, wordWrap = true };
                small = new GUIStyle(GUI.skin.label) { fontSize = 14, wordWrap = true };
            }
            // A fixed design canvas keeps instructions readable across window sizes.
            Matrix4x4 previous = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(Screen.width / 1280f, Screen.height / 720f, 1));
            GUI.Box(new Rect(18, 16, 385, 100), GUIContent.none);
            GUI.Label(new Rect(32, 23, 360, 32), "PROJECT TERRA / FIRST COMBAT", text);
            Bar(new Rect(32, 62, 285, 18), Session.PlayerHealth / 100f, new Color(.75f, .24f, .29f));
            GUI.Label(new Rect(325, 58, 80, 25), Session.PlayerHealth + " HP", small);
            GUI.Label(new Rect(32, 85, 360, 25), "Dash: " + (Session.DashCooldownRemaining <= 0 ? "ready" : Session.DashCooldownRemaining.ToString("0.0") + "s") + "   |   Earth seed: " + (Session.SeedCollected ? "1 / 3" : "0 / 3"), small);

            if (Session.InArena && Session.State == EncounterState.Fighting)
            {
                GUI.Box(new Rect(440, 16, 500, 95), GUIContent.none);
                GUI.Label(new Rect(460, 22, 470, 30), "THE CAIRN WARDEN", text);
                Bar(new Rect(460, 56, 460, 14), Session.BossHealth / 180f, new Color(.70f, .50f, .22f));
                string tell = Session.Phase == BossPhase.Recovery ? "RECOVERY — strike now" :
                    Session.Move == BossMove.Faultline ? "FAULTLINE — leave the marked ground" : "QUAKE — jump the travelling wave";
                GUI.Label(new Rect(460, 77, 470, 28), tell, small);
            }
            GUI.Box(new Rect(18, 625, 1244, 78), GUIContent.none);
            GUI.Label(new Rect(34, 635, 1210, 27), "A/D or arrows: move   •   Space: jump (release for short hop)   •   LMB / J: sword   •   Shift / RMB: dash   •   E: collect   •   Esc: pause", small);
            string objective = Session.State == EncounterState.Village ? "The village is quiet. Walk east to challenge the Earth guardian." :
                Session.State == EncounterState.Fighting ? "Watch the warning. Dodge the faultline, jump the quake, punish the recovery. Dash grants no invulnerability." :
                Session.State == EncounterState.Dead ? "Your attempt has ended. R starts a fresh practice attempt in the village." :
                Session.SeedCollected ? "Earth seed collected (1 of 3). Return west to the village. This prototype ends here; R starts a fresh practice attempt." :
                "Guardian defeated. Approach the glowing seed and press E. The arena exit is now open.";
            GUI.Label(new Rect(34, 665, 1210, 32), objective, small);
            if (Session.Paused || Session.State == EncounterState.Dead)
            {
                GUI.Box(new Rect(410, 245, 460, 210), GUIContent.none);
                GUI.Label(new Rect(442, 265, 400, 40), Session.Paused ? "PAUSED" : "THE WORLD FALLS SILENT", title);
                GUI.Label(new Rect(442, 312, 395, 60), Session.Paused ? "Take a breath. Resume when ready." : "No seed progress survives death. Try again with what you learned.", text);
                if (Session.Paused && GUI.Button(new Rect(442, 390, 185, 40), "Resume [Esc]")) TogglePause();
                if (Session.State == EncounterState.Dead && GUI.Button(new Rect(642, 390, 195, 40), "New attempt [R]")) NewAttempt();
            }
            GUI.matrix = previous;
        }

        private static void Bar(Rect rect, float amount, Color color)
        {
            Color previous = GUI.color;
            GUI.color = new Color(.12f, .12f, .16f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(amount), rect.height), Texture2D.whiteTexture);
            GUI.color = previous;
        }
    }
}
