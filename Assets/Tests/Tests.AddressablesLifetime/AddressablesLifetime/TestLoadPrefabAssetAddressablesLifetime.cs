using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Entity;
using EntityHolder;

namespace Tests.AddressablesLifetime
{
    public class TestLoadPrefabAssetAddressablesLifetime : TestAddressablesLifetimeBase
    {
        private AsyncOperationHandle<GameObject> _handle = default;
        private GameObject _gameObject = null;

        protected override IEnumerator Preload()
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(key: PrefabAddressablesLabel);
            _handle.WaitForCompletion();

            yield return null;
            Assert.That(_handle.IsDone, Is.True);
        }

        protected override IEntity LoadTarget()
        {
            _gameObject = Object.Instantiate(_handle.Result);
            MonoBehaviourEntityHolder entityHolder = _gameObject.GetComponent<MonoBehaviourEntityHolder>();
            IEntity entity = entityHolder.Entity;
            Assert.That(entity.Identifier, Is.EqualTo(EntityIdentifier));
            return entity;
        }

        protected override void UnoadTarget()
        {
            Object.DestroyImmediate(_gameObject);
            _gameObject = null;

            Addressables.Release(_handle);
            _handle = default;
        }
    }
}
