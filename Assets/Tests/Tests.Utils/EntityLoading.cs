using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Entity;
using EntityHolder;

namespace Tests.Utils
{
    public sealed class EntityLoading
    {
        public static IEntity LoadEntity(Scene scene, string holderObjectName)
        {
            var objects = scene.GetRootGameObjects();
            foreach (var obj in objects)
            {
                var entity = LoadEntityInternal(obj, holderObjectName);
                if (entity != null)
                {
                    return entity;
                }
            }

            throw new NotFoundEntityException();
        }

        private static IEntity LoadEntityInternal(GameObject go, string name)
        {
            if (go.name != name)
            {
                return null;
            }

            var holder = go.GetComponent<IEntityHolder>();
            if (holder == null)
            {
                return null;
            }

            return holder.Entity;
        }

        [SetUp]
        public void SetUp()
        {
            SceneLoading.UnloadAllScenes();
        }

        [TearDown]
        public void TearDown()
        {
            SceneLoading.UnloadAllScenes();
        }

        [UnityTest]
        public IEnumerator LoadEntity_ReturnsInstanceIfTargetExists()
        {
            // setup
            string sceneName = "TestWithTargetObject";

            var parameters = new LoadSceneParameters(mode: LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(sceneName, parameters: parameters);
            var scene = SceneManager.GetSceneByName(sceneName);

            // when
            var actual = LoadEntity(scene, TestTargetObjectName);

            // then
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Identifier, Is.EqualTo(TestTargetIdentifier));
        }

        [UnityTest]
        public IEnumerator LoadEntity_ThrowsExceptionIfTargetDoesNotExist()
        {
            // setup
            string sceneName = "TestWithoutTargetObject";

            var parameters = new LoadSceneParameters(mode: LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(sceneName, parameters: parameters);
            var scene = SceneManager.GetSceneByName(sceneName);

            // when, then
            Assert.Throws<NotFoundEntityException>(() =>
            {
                LoadEntity(scene, TestTargetObjectName);
            });
        }

        [UnityTest]
        public IEnumerator LoadEntity_ThrowsExceptionIfTargetIsNotFound()
        {
            // setup
            string sceneName = "TestWithAnotherObject";

            var parameters = new LoadSceneParameters(mode: LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(sceneName, parameters: parameters);
            var scene = SceneManager.GetSceneByName(sceneName);

            // when, then
            Assert.Throws<NotFoundEntityException>(() =>
            {
                LoadEntity(scene, TestTargetObjectName);
            });
        }

        [UnityTest]
        public IEnumerator LoadEntity_ThrowsExceptionIfTargetDoesNotHaveEntity()
        {
            // setup
            string sceneName = "TestWithoutTargetEntity";

            var parameters = new LoadSceneParameters(mode: LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(sceneName, parameters: parameters);
            var scene = SceneManager.GetSceneByName(sceneName);

            // when, then
            Assert.Throws<NotFoundEntityException>(() =>
            {
                LoadEntity(scene, TestTargetObjectName);
            });
        }

        const string TestTargetObjectName = "TargetObject";
        const string TestTargetIdentifier = "__TEST__";
    }
}
