using System;

namespace Terra.Core
{
    public enum EncounterState { Village, Fighting, Victory, Dead }
    public enum BossPhase { Telegraph, Active, Recovery, Defeated }
    public enum BossMove { Faultline, Quake }

    // Input is a command snapshot, independent of Unity, keyboard, or future controller bindings.
    public struct CombatInput
    {
        public float Move;
        public bool JumpPressed, JumpReleased, AttackPressed, DashPressed, InteractPressed;
    }

    /// <summary>Provisional feel values. These are playtest parameters, not approved campaign rules.</summary>
    public static class CombatTuning
    {
        public const float Speed = 8f, JumpSpeed = 15f, Gravity = 40f;
        public const float JumpBuffer = .12f, AttackBuffer = .12f;
        public const float SwingDuration = .30f, SwingActiveStart = .06f, SwingActiveEnd = .16f;
        public const float SwordReach = 2.6f, DashSpeed = 19f, DashDuration = .14f, DashCooldown = .8f;
        public const int MaxPlayerHealth = 100, MaxBossHealth = 180, SwordDamage = 20, BossDamage = 25;
        public const float HurtGrace = .7f, BossX = 17f;
    }

    /// <summary>
    /// Authoritative first-fight simulation. Coordinates are feet positions in world units.
    /// The prototype has a flat floor and two connected rooms; it is not the eventual terrain system.
    /// Advance in fixed steps of at most 1/60 s so narrow active windows cannot be skipped.
    /// </summary>
    public sealed class CombatSession
    {
        public EncounterState State { get; private set; }
        public BossPhase Phase { get; private set; }
        public BossMove Move { get; private set; }
        public float PlayerX { get; private set; }
        public float PlayerY { get; private set; }
        public float VelocityY { get; private set; }
        public int Facing { get; private set; }
        public int PlayerHealth { get; private set; }
        public int BossHealth { get; private set; }
        public float PhaseRemaining { get; private set; }
        public float TargetX { get; private set; }
        public float WaveX { get; private set; }
        public int WaveDirection { get; private set; }
        public bool WaveActive { get; private set; }
        public bool SeedCollected { get; private set; }
        public bool Paused { get; private set; }
        public float HurtRemaining { get; private set; }
        public float DashCooldownRemaining { get; private set; }
        public bool Grounded => PlayerY <= 0f;
        public bool Dashing => dashRemaining > 0f;
        public bool Swinging => swingElapsed >= 0f;
        public bool SwordActive => Swinging && swingElapsed >= CombatTuning.SwingActiveStart && swingElapsed <= CombatTuning.SwingActiveEnd;
        public bool InArena => PlayerX >= 0f;
        public bool SeedAvailable => State == EncounterState.Victory && !SeedCollected;

        private float jumpBuffer, attackBuffer, swingElapsed = -1f, dashRemaining;
        private bool swingHit;
        private int dashDirection, attackDirection, moveIndex;
        public int AttackDirection => attackDirection;

        public CombatSession() { Reset(); }

        public void Reset()
        {
            State = EncounterState.Village;
            Phase = BossPhase.Recovery;
            Move = BossMove.Faultline;
            PlayerX = -12f;
            PlayerY = VelocityY = 0f;
            Facing = attackDirection = dashDirection = 1;
            PlayerHealth = CombatTuning.MaxPlayerHealth;
            BossHealth = CombatTuning.MaxBossHealth;
            PhaseRemaining = 1f;
            SeedCollected = Paused = WaveActive = swingHit = false;
            HurtRemaining = DashCooldownRemaining = dashRemaining = jumpBuffer = attackBuffer = 0f;
            swingElapsed = -1f;
            moveIndex = 0;
            TargetX = WaveX = CombatTuning.BossX;
            WaveDirection = -1;
        }

        public void SetPaused(bool paused)
        {
            Paused = paused;
            // A click made in a menu must not become a sword swing after resuming.
            jumpBuffer = attackBuffer = 0f;
        }

        public void Step(float dt, CombatInput input)
        {
            if (dt <= 0f || dt > 1f / 60f + .00001f || float.IsNaN(dt))
                throw new ArgumentOutOfRangeException(nameof(dt), "Use a positive fixed timestep <= 1/60 second.");
            if (Paused || State == EncounterState.Dead) return;
            HurtRemaining = Down(HurtRemaining, dt);
            DashCooldownRemaining = Down(DashCooldownRemaining, dt);
            jumpBuffer = input.JumpPressed ? CombatTuning.JumpBuffer : Down(jumpBuffer, dt);
            attackBuffer = input.AttackPressed ? CombatTuning.AttackBuffer : Down(attackBuffer, dt);
            float axis = float.IsNaN(input.Move) ? 0f : Clamp(input.Move, -1f, 1f);
            if (axis != 0f && !Dashing) Facing = axis > 0f ? 1 : -1;

            if (jumpBuffer > 0f && Grounded)
            {
                VelocityY = CombatTuning.JumpSpeed;
                jumpBuffer = 0f;
            }
            // Release cuts ascent once, permitting both short hops and full jumps.
            if (input.JumpReleased && VelocityY > 0f) VelocityY *= .45f;
            if (input.DashPressed && DashCooldownRemaining <= 0f && !Swinging)
            {
                dashRemaining = CombatTuning.DashDuration;
                DashCooldownRemaining = CombatTuning.DashCooldown;
                dashDirection = Facing;
            }
            float velocityX = Dashing ? dashDirection * CombatTuning.DashSpeed : axis * CombatTuning.Speed;
            PlayerX += velocityX * dt;
            dashRemaining = Down(dashRemaining, dt);
            VelocityY = Math.Max(VelocityY - CombatTuning.Gravity * dt, -22f);
            PlayerY += VelocityY * dt;
            if (PlayerY <= 0f) { PlayerY = 0f; VelocityY = 0f; }
            PlayerX = Clamp(PlayerX, State == EncounterState.Fighting ? .6f : -22f, 25f);
            if (State == EncounterState.Village && PlayerX >= 0f)
            {
                State = EncounterState.Fighting;
                PlayerX = Math.Max(.6f, PlayerX);
            }

            TickSword(dt);
            if (State == EncounterState.Fighting) TickBoss(dt);
            if (SeedAvailable && input.InteractPressed && Math.Abs(PlayerX - CombatTuning.BossX) < 1.8f && PlayerY < 2f)
                SeedCollected = true;
        }

        private void TickSword(float dt)
        {
            if (!Swinging && !Dashing && attackBuffer > 0f)
            {
                swingElapsed = 0f;
                swingHit = false;
                attackDirection = Facing; // Swing direction commits at startup; movement remains responsive.
                attackBuffer = 0f;
            }
            if (!Swinging) return;
            swingElapsed += dt;
            float distance = (CombatTuning.BossX - PlayerX) * attackDirection;
            if (State == EncounterState.Fighting && SwordActive && !swingHit && distance >= -.7f && distance <= CombatTuning.SwordReach + .8f && PlayerY < 3f)
            {
                BossHealth = Math.Max(0, BossHealth - CombatTuning.SwordDamage);
                swingHit = true; // One target can be damaged only once by a single swing.
                if (BossHealth == 0)
                {
                    State = EncounterState.Victory;
                    Phase = BossPhase.Defeated;
                    WaveActive = false; // No lingering hazards after encounter completion.
                }
            }
            if (swingElapsed >= CombatTuning.SwingDuration) swingElapsed = -1f;
        }

        private void TickBoss(float dt)
        {
            PhaseRemaining -= dt;
            if (PhaseRemaining <= 0f)
            {
                // Carry timing overshoot into the next phase to avoid cumulative frame-rate drift.
                if (Phase == BossPhase.Recovery)
                {
                    Move = moveIndex++ % 2 == 0 ? BossMove.Faultline : BossMove.Quake;
                    TargetX = Clamp(PlayerX, 2.5f, 23f); // Aim locks when the warning begins.
                    WaveDirection = PlayerX < CombatTuning.BossX ? -1 : 1;
                    Phase = BossPhase.Telegraph;
                    PhaseRemaining += Move == BossMove.Faultline ? .9f : .8f;
                }
                else if (Phase == BossPhase.Telegraph)
                {
                    Phase = BossPhase.Active;
                    PhaseRemaining += Move == BossMove.Faultline ? .22f : 1.8f;
                    if (Move == BossMove.Quake) { WaveX = CombatTuning.BossX; WaveActive = true; }
                }
                else
                {
                    WaveActive = false;
                    Phase = BossPhase.Recovery;
                    PhaseRemaining += 1.1f;
                }
            }
            if (Phase != BossPhase.Active) return;
            if (Move == BossMove.Faultline && Math.Abs(PlayerX - TargetX) <= 2.25f && PlayerY < 1.5f)
                HurtPlayer();
            if (Move == BossMove.Quake && WaveActive)
            {
                WaveX += WaveDirection * 12f * dt;
                if (Math.Abs(PlayerX - WaveX) <= .85f && PlayerY < .9f) HurtPlayer();
                if (WaveX < 0f || WaveX > 26f) WaveActive = false;
            }
        }

        private void HurtPlayer()
        {
            if (HurtRemaining > 0f) return;
            PlayerHealth = Math.Max(0, PlayerHealth - CombatTuning.BossDamage);
            HurtRemaining = CombatTuning.HurtGrace;
            if (PlayerHealth > 0) return;
            State = EncounterState.Dead;
            SeedCollected = false;
            WaveActive = false;
            swingElapsed = -1f;
            jumpBuffer = attackBuffer = dashRemaining = 0f;
        }

        private static float Down(float value, float dt) => Math.Max(0f, value - dt);
        private static float Clamp(float value, float min, float max) => Math.Max(min, Math.Min(max, value));
    }
}
