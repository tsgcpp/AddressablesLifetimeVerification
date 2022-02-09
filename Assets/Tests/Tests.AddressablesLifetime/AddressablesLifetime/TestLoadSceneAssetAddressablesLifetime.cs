/*
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Entity;
using Tests.Utils;

namespace Tests.AddressablesLifetime
{
    public class TestLoadSceneAssetAddressablesLifetime : TestAddressablesLifetimeBase
    {
        private AsyncOperationHandle<SceneInstance> _handle = default;

        protected override IEnumerator Preload()
        {
            string sceneName = SceneBuiltinObjectInAddressablesScene;
            _handle = Addressables.LoadSceneAsync(sceneName, loadMode: LoadSceneMode.Single);
            _handle.WaitForCompletion();
            var ao = _handle.Result.ActivateAsync();

            yield return new WaitForSeconds(WaitForSceneLoaded);
            Assert.That(ao.isDone, Is.True);
        }

        protected override IEntity LoadTarget()
        {
            IEntity entity = EntityLoading.LoadEntity(_handle.Result.Scene, TestTargetObjectName);
            Assert.That(entity.Identifier, Is.EqualTo(EntityIdentifier));
            return entity;
        }

        protected override void UnoadTarget()
        {
            _handle = default;
            // Unload by SceneLoading.UnloadAllScenes() in TearDown()
        }

        const float WaitForSceneLoaded = 0.1f;
    }
}
*/
