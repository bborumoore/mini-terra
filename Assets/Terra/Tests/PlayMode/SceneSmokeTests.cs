using System.Collections;
using NUnit.Framework;
using Terra.Core;
using Terra.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Terra.Tests
{
    public sealed class SceneSmokeTests
    {
        [UnityTest]
        public IEnumerator CommittedSceneCreatesVillageCameraAndHero()
        {
            yield return SceneManager.LoadSceneAsync("FirstCombat", LoadSceneMode.Single);
            yield return null;
            var bootstrap = Object.FindFirstObjectByType<TerraBootstrap>();
            Assert.That(bootstrap, Is.Not.Null);
            Assert.That(bootstrap.Session.State, Is.EqualTo(EncounterState.Village));
            Assert.That(Camera.main, Is.Not.Null);
            Assert.That(Camera.main.orthographic, Is.True);
            Assert.That(GameObject.Find("Hero"), Is.Not.Null);
            Assert.That(GameObject.Find("Cairn Warden"), Is.Not.Null);
            LogAssert.NoUnexpectedReceived();
        }
    }
}
