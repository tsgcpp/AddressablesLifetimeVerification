using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Entity;
using EntityHolder;
using Tests.Utils;

namespace Tests.AddressablesLifetime
{
    public abstract class TestAddressablesLifetimeBase
    {
        private IEntity _target = null;

        protected abstract IEnumerator Preload();

        protected abstract IEntity LoadTarget();

        protected abstract void UnoadTarget();

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return Preload();
            _target = LoadTarget();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            UnoadTarget();
            _target = null;
            yield return SceneLoading.UnloadAllScenes();
            yield return null;
        }


        [UnityTest]
        public IEnumerator AddressablesLifetime_AssetInAddressablesIsSingleInstanceIfPreloaded()
        {
            // setup
            Assert.That(_target, Is.Not.Null);
            yield return null;

            // 1st load and unload
            yield return AssertAddressablesAssetsAreOnlySameInstance();

            // 2nd load and unload
            yield return new WaitForSeconds(WaitSecondsPerCycle);
            yield return AssertAddressablesAssetsAreOnlySameInstance();

            // 3rd load and unload
            yield return new WaitForSeconds(WaitSecondsPerCycle);
            yield return AssertAddressablesAssetsAreOnlySameInstance();
        }

        private IEnumerator AssertAddressablesAssetsAreOnlySameInstance()
        {
            // Addressables assets, prefabs and scenes
            yield return AssertEntityInAddressablesScene(SceneBuiltinObjectInAddressablesScene, Is.EqualTo(_target));
            yield return AssertEntityInAddressablesScene(SceneBuiltinPrefabInAddressablesScene, Is.EqualTo(_target));
            yield return AssertEntityInAddressablesScene(SceneAddressablesPrefabInAddressablesScene, Is.EqualTo(_target));
            yield return AssertEntityInAddressablesPrefab(PrefabAddressablesLabel, Is.EqualTo(_target));

            // Buildin assets, prefabs and scenes
            yield return AssertEntityInBuiltinScene(SceneBuiltinObjectInBuiltinScene, Is.Not.EqualTo(_target));
            yield return AssertEntityInBuiltinScene(SceneBuiltinPrefabInBuiltinScene, Is.Not.EqualTo(_target));
            yield return AssertEntityInBuiltinScene(SceneAddressablesPrefabInBuiltinScene, Is.Not.EqualTo(_target));
            yield return AssertEntityInResourcesPrefab(PrefabResourcesName, Is.Not.EqualTo(_target));
        }

        private IEnumerator AssertEntityInBuiltinScene(string sceneName, EqualConstraint constraint)
        {
            // setup
            var loadSceneParameters = new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Single,
            };
            var scene = SceneManager.LoadScene(sceneName, parameters: loadSceneParameters);
            yield return null;

            IEntity entity = EntityLoading.LoadEntity(scene, TestTargetObjectName);

            // when, then
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity, constraint);

            // teardown
            yield return SceneLoading.UnloadAllScenes();
        }

        private IEnumerator AssertEntityInAddressablesScene(string sceneName, EqualConstraint constraint)
        {
            // setup
            var op = Addressables.LoadSceneAsync(sceneName, loadMode: LoadSceneMode.Single);
            yield return op;

            IEntity entity = EntityLoading.LoadEntity(op.Result.Scene, TestTargetObjectName);

            // when, then
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity, constraint);

            // teardown
            // Addressables scene will also be released
            yield return SceneLoading.UnloadAllScenes();
        }

        private IEnumerator AssertEntityInResourcesPrefab(string resourcesName, EqualConstraint constraint)
        {
            // setup
            var prefab = Resources.Load<MonoBehaviourEntityHolder>(resourcesName);
            yield return null;

            MonoBehaviourEntityHolder entityHolder = Object.Instantiate(prefab);
            IEntity entity = entityHolder.Entity;

            // when, then
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity, constraint);

            // teardown
            Object.DestroyImmediate(entityHolder);
            yield return Resources.UnloadUnusedAssets();
        }

        private IEnumerator AssertEntityInAddressablesPrefab(string key, EqualConstraint constraint)
        {
            // setup
            var op = Addressables.LoadAssetAsync<GameObject>(key: key);
            yield return op;

            var go = Object.Instantiate(op.Result);
            MonoBehaviourEntityHolder entityHolder = go.GetComponent<MonoBehaviourEntityHolder>();
            IEntity entity = entityHolder.Entity;


            // when, then
            Assert.That(entity, Is.Not.Null);
            Assert.That(entity, constraint);

            // teardown
            Object.DestroyImmediate(entityHolder);
            Addressables.Release(op);
            yield return null;
        }

        public const string EntityIdentifier = "__TARGET__";

        public const string TestTargetObjectName = "TargetObject";

        public const string PrefabResourcesName = "TargetObjectResource";
        public const string PrefabAddressablesLabel = "TargetObjectAddressables";

        public const string SceneBuiltinObjectInBuiltinScene = "BuiltinObjectInBuiltinScene";
        public const string SceneBuiltinPrefabInBuiltinScene = "BuiltinPrefabInBuiltinScene";
        public const string SceneAddressablesPrefabInBuiltinScene = "AddressablesPrefabInBuiltinScene";
        public const string SceneBuiltinObjectInAddressablesScene = "BuiltinObjectInAddressablesScene";
        public const string SceneBuiltinPrefabInAddressablesScene = "BuiltinPrefabInAddressablesScene";
        public const string SceneAddressablesPrefabInAddressablesScene = "AddressablesPrefabInAddressablesScene";

        public const float WaitSecondsPerCycle = 0.1f;
    }
}
