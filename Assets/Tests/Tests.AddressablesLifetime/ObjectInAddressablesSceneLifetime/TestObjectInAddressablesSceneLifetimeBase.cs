using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Entity;
using EntityHolder;
using Tests.Utils;

namespace Tests.AddressablesLifetime
{
    public abstract class TestScriptableObjectInAddressablesSceneLifetime
    {
        private IEntity _target = null;
        private AsyncOperationHandle<SceneInstance> _handle = default;

        protected abstract string TargetSceneName { get; }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            string sceneName = TargetSceneName;
            _handle = Addressables.LoadSceneAsync(sceneName, loadMode: LoadSceneMode.Single);
            _handle.WaitForCompletion();
            yield return _handle.Result.ActivateAsync();

            _target = EntityLoading.LoadEntity(_handle.Result.Scene, TestTargetObjectName);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            _target = null;
            _handle = default;
            yield return SceneLoading.UnloadAllScenes();
            yield return null;
        }

        [UnityTest]
        public IEnumerator AddressablesLifetime_AssetInAddressablesSceneWillBeReleasedAfterTheSceneIsReleased()
        {
            Assert.That(_target, Is.Not.Null);
            Assert.That(_target.Identifier, Is.EqualTo(EntityIdentifier));
            yield return null;

            // when
            yield return SceneLoading.UnloadAllScenes();

            // then
            // Assert _target as UnityEngine.Object
            Assert.That(_target.Equals(null), Is.True);
        }

        const string EntityIdentifier = "__TARGET__";
        const string TestTargetObjectName = "TargetObject";
        const float WaitForSceneLoaded = 0.1f;
    }
}
