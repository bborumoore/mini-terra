using System;
using Terra.Core;

namespace Terra.Tests
{
    // The same specifications execute under NUnit in Unity and the small .NET console runner.
    // Tests drive commands; they do not modify private state to force a desired outcome.
    public static class CombatSpecifications
    {
        private const float Dt = 1f / 120f;
        public static void InitialVillage()
        {
            var s = new CombatSession();
            Check(s.State == EncounterState.Village && s.PlayerX < 0 && s.Grounded, "Start safely in village");
            Advance(s, 10);
            Check(s.PlayerHealth == 100 && s.BossHealth == 180, "Village has no hostile simulation");
        }
        public static void ImmediateMovementAndStop()
        {
            var s = new CombatSession(); float x = s.PlayerX;
            s.Step(Dt, new CombatInput { Move = 1 });
            Check(s.PlayerX > x, "Movement starts on first tick");
            x = s.PlayerX; s.Step(Dt, default);
            Check(s.PlayerX == x, "No unwanted ground inertia");
        }
        public static void VariableJumpHeight()
        {
            float high = JumpPeak(false), low = JumpPeak(true);
            Check(high > low + 1f && low > 0f, "Early release produces a shorter controllable jump");
        }
        public static void JumpBuffersBeforeLanding()
        {
            var s = new CombatSession();
            s.Step(Dt, new CombatInput { JumpPressed = true });
            for (int i = 0; i < 200; i++)
            {
                if (s.PlayerY < .2f && s.VelocityY < 0)
                {
                    s.Step(Dt, new CombatInput { JumpPressed = true });
                    Advance(s, .08f);
                    Check(s.PlayerY > .2f && s.VelocityY > 0, "Pre-landing press produces a new jump");
                    return;
                }
                s.Step(Dt, default);
            }
            throw new Exception("Landing not reached");
        }
        public static void ArenaLocksUntilOutcome()
        {
            var s = Enter(); Advance(s, .5f, -1);
            Check(s.State == EncounterState.Fighting && s.PlayerX >= .6f, "Cannot bypass active encounter by leaving west");
        }
        public static void TelegraphLocksAim()
        {
            var s = Enter(); WaitFor(s, BossPhase.Telegraph, BossMove.Faultline);
            float target = s.TargetX; Advance(s, .3f, 1);
            Check(s.TargetX == target, "Warning must not secretly track the player's dodge");
        }
        public static void WarningCannotDamageAndFaultlineCanBeDodged()
        {
            var s = Enter(); WaitFor(s, BossPhase.Telegraph, BossMove.Faultline);
            int hp = s.PlayerHealth;
            Advance(s, .6f, 1);
            Check(s.PlayerHealth == hp, "Telegraph is a warning, not a damaging hitbox");
            Advance(s, .65f);
            Check(s.PlayerHealth == hp, "Leaving the locked marker avoids faultline damage");
        }
        public static void QuakeCanBeJumped()
        {
            var s = Enter(); MoveTo(s, 10f);
            WaitFor(s, BossPhase.Telegraph, BossMove.Quake);
            int hp = s.PlayerHealth;
            Advance(s, .78f);
            s.Step(Dt, new CombatInput { JumpPressed = true });
            Advance(s, .65f);
            Check(s.PlayerHealth == hp, "A well-timed full jump clears the ground wave");
        }
        public static void DamageGraceAndDeath()
        {
            var s = Enter(); MoveTo(s, 5f);
            for (int i = 0; i < 6000 && s.State != EncounterState.Dead; i++) s.Step(Dt, default);
            Check(s.State == EncounterState.Dead && s.PlayerHealth == 0, "Standing in repeated attacks eventually ends attempt");
            float x = s.PlayerX; s.Step(Dt, new CombatInput { Move = 1, AttackPressed = true, JumpPressed = true });
            Check(s.PlayerX == x && !s.SeedCollected && !s.WaveActive, "Death disables combat and seed progress");
            s.Reset(); Check(s.State == EncounterState.Village && s.PlayerHealth == 100 && s.BossHealth == 180, "Retry resets encounter");
        }
        public static void OneHitPerFaultline()
        {
            var s = Enter(); MoveTo(s, 5f);
            WaitFor(s, BossPhase.Telegraph, BossMove.Faultline);
            int hp = s.PlayerHealth;
            Advance(s, 1.2f);
            Check(s.PlayerHealth == hp - CombatTuning.BossDamage, "Active multi-frame faultline inflicts one hit, not one per tick");
        }
        public static void PauseFreezesAndDiscardsBufferedActions()
        {
            var s = Enter(); float x = s.PlayerX, phase = s.PhaseRemaining;
            s.SetPaused(true);
            for (int i = 0; i < 300; i++) s.Step(Dt, new CombatInput { Move = 1, JumpPressed = true, AttackPressed = true });
            Check(s.PlayerX == x && s.PhaseRemaining == phase && s.PlayerHealth == 100, "Pause freezes all gameplay timers and motion");
            s.SetPaused(false); s.Step(Dt, default);
            Check(s.Grounded && !s.Swinging, "Paused commands cannot leak into resume");
        }
        public static void OneHitPerSwordSwing()
        {
            var s = Enter(); MoveTo(s, 14f);
            int hp = s.BossHealth;
            s.Step(Dt, new CombatInput { AttackPressed = true }); Advance(s, .3f);
            Check(s.BossHealth == hp - CombatTuning.SwordDamage, "One swing must not hit on every active tick");
        }
        public static void SwordRespectsFacingAndRange()
        {
            var s = Enter();
            s.Step(Dt, new CombatInput { AttackPressed = true }); Advance(s, .4f);
            Check(s.BossHealth == 180, "Cannot hit across the arena");
            MoveTo(s, 14f); s.Step(Dt, new CombatInput { Move = -1 });
            s.Step(Dt, new CombatInput { AttackPressed = true }); Advance(s, .3f);
            Check(s.BossHealth == 180, "A left-facing sword cannot hit the boss to the right");
        }
        public static void DashHasCooldown()
        {
            var s = new CombatSession();
            s.Step(Dt, new CombatInput { DashPressed = true }); Advance(s, .2f);
            Check(!s.Dashing && s.DashCooldownRemaining > 0, "Dash stops before its cooldown expires");
            float x = s.PlayerX; s.Step(Dt, new CombatInput { DashPressed = true });
            Check(!s.Dashing && s.PlayerX == x, "Repeated button press does not bypass cooldown");
        }
        public static void CompleteEncounterAndCollectOnce()
        {
            var s = Win();
            Check(s.SeedAvailable && !s.SeedCollected && !s.WaveActive && s.Phase == BossPhase.Defeated, "Victory clears hazards and drops a seed");
            MoveTo(s, CombatTuning.BossX);
            s.Step(Dt, new CombatInput { InteractPressed = true });
            Check(s.SeedCollected && !s.SeedAvailable, "Nearby interaction collects the seed");
            s.Step(Dt, new CombatInput { InteractPressed = true });
            Check(s.SeedCollected && s.State == EncounterState.Victory, "Collection neither duplicates nor ends a three-seed run early");
            MoveTo(s, -2f);
            Check(!s.InArena, "Victory releases the village exit");
            s.Reset(); Check(!s.SeedCollected, "New practice attempt resets the local seed demonstration");
        }
        public static void SeedRequiresProximity()
        {
            var s = Win(); MoveTo(s, 2f);
            s.Step(Dt, new CombatInput { InteractPressed = true });
            Check(!s.SeedCollected, "Cannot remotely collect the seed");
        }
        public static void RejectsUnsafeTimestep()
        {
            bool rejected = false;
            try { new CombatSession().Step(.5f, default); } catch (ArgumentOutOfRangeException) { rejected = true; }
            Check(rejected, "Large ticks cannot skip telegraphs or hit windows");
        }
        public static void ReplayIsDeterministic()
        {
            var a = new CombatSession(); var b = new CombatSession();
            for (int i = 0; i < 2400; i++)
            {
                var command = new CombatInput { Move = i < 400 ? 1 : 0, AttackPressed = i % 42 == 0, JumpPressed = i % 130 == 0 };
                a.Step(Dt, command); b.Step(Dt, command);
            }
            Check(a.PlayerX == b.PlayerX && a.PlayerY == b.PlayerY && a.PlayerHealth == b.PlayerHealth && a.BossHealth == b.BossHealth && a.State == b.State, "Identical command streams must yield identical outcomes");
        }

        private static CombatSession Win()
        {
            var s = Enter(); MoveTo(s, 15f);
            // Scripted player attacks and jumps without changing private simulation state.
            for (int i = 0; i < 4000 && s.State == EncounterState.Fighting; i++)
                s.Step(Dt, new CombatInput { AttackPressed = i % 40 == 0, JumpPressed = s.Grounded });
            Check(s.State == EncounterState.Victory && s.PlayerHealth > 0, "An input-only playthrough must be winnable");
            while (!s.Grounded) s.Step(Dt, default);
            return s;
        }
        private static float JumpPeak(bool earlyRelease)
        {
            var s = new CombatSession(); s.Step(Dt, new CombatInput { JumpPressed = true }); float peak = s.PlayerY;
            for (int i = 0; i < 150; i++)
            { s.Step(Dt, new CombatInput { JumpReleased = earlyRelease && i == 5 }); peak = Math.Max(peak, s.PlayerY); }
            Check(s.Grounded, "Jump must land back on floor"); return peak;
        }
        private static CombatSession Enter()
        { var s = new CombatSession(); while (s.State == EncounterState.Village) s.Step(Dt, new CombatInput { Move = 1 }); return s; }
        private static void MoveTo(CombatSession s, float x)
        {
            for (int i = 0; i < 1000 && Math.Abs(s.PlayerX - x) > .08f; i++)
                s.Step(Dt, new CombatInput { Move = s.PlayerX < x ? 1 : -1 });
            Check(Math.Abs(s.PlayerX - x) <= .08f, "Reach target position " + x);
        }
        private static void WaitFor(CombatSession s, BossPhase phase, BossMove move)
        {
            for (int i = 0; i < 2000; i++)
            { if (s.Phase == phase && s.Move == move) return; s.Step(Dt, default); }
            throw new Exception("Expected phase not reached");
        }
        private static void Advance(CombatSession s, float seconds, float axis = 0)
        { for (int i = 0; i < (int)Math.Ceiling(seconds / Dt); i++) s.Step(Dt, new CombatInput { Move = axis }); }
        private static void Check(bool condition, string reason) { if (!condition) throw new Exception(reason); }
    }
}
