using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Entity;

namespace Tests.AddressablesLifetime
{
    public class TestPreloadAssetAddressablesLifetime : TestAddressablesLifetimeBase
    {
        private AsyncOperationHandle<IList<ScriptableObjectEntity>> _handle = default;

        protected override IEnumerator Preload()
        {
            _handle = Addressables.LoadAssetsAsync<ScriptableObjectEntity>(key: TargetLabel, callback: null);
            _handle.WaitForCompletion();

            yield return null;
            Assert.That(_handle.IsDone, Is.True);
        }

        protected override IEntity LoadTarget()
        {
            ScriptableObjectEntity entity = _handle.Result[0];
            Assert.That(entity.Identifier, Is.EqualTo(EntityIdentifier));
            return entity;
        }

        protected override void UnoadTarget()
        {
            Addressables.Release(_handle);
            _handle = default;
        }

        const string TargetLabel = "LabelTarget";
    }
}
