using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.Utils
{
    public sealed class SceneLoading
    {
        public static IEnumerator UnloadAllScenes()
        {
            yield return SceneManager.LoadSceneAsync(EmtpySceneName, mode: LoadSceneMode.Single);
        }

        [UnityTest]
        public IEnumerator LoadEntity_ReturnsInstanceIfTargetExists()
        {
            // setup
            yield return SceneManager.LoadSceneAsync(EmtpySceneName, mode: LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(EmtpySceneName, mode: LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync(EmtpySceneName, mode: LoadSceneMode.Additive);
            Assert.That(SceneManager.sceneCount, Is.EqualTo(3));

            // when
            yield return UnloadAllScenes();

            // then
            Assert.That(SceneManager.sceneCount, Is.EqualTo(1));
            Assert.That(SceneManager.GetSceneAt(0).name, Is.EqualTo(EmtpySceneName));
        }

        const string EmtpySceneName = "Empty";
    }
}
