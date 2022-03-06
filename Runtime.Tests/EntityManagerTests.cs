using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Tests
{
    public class EntityManagerTests
    {
        [Test]
        public void RegisterSimple()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            HumbleEntity entity = entityManager.CreateAndRegisterEntity();

            Assert.IsTrue(entityManager.HasEntity(entity));
        }

        [Test]
        public void UnRegisterSimple()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            HumbleEntity entity = entityManager.CreateAndRegisterEntity();
            entityManager.UnRegister(entity);

            Assert.IsFalse(entityManager.HasEntity(entity));
        }

        [Test]
        public void TicksAllEntities()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            List<HumbleEntity> entities = new List<HumbleEntity>
            {
                entityManager.CreateAndRegisterEntity(),
                entityManager.CreateAndRegisterEntity(),
            };

            Assert.IsFalse(entities.All(element => element.TickedAtLeastOnce));

            entityManager.Tick(.5f);

            Assert.IsTrue(entities.All(element => element.TickedAtLeastOnce));
        }

        [Test]
        public void DisposeEntityRemovesItFromList()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            HumbleEntity entity = entityManager.CreateAndRegisterEntity();
            entityManager.DisposeEntity(entity);

            Assert.IsFalse(entityManager.HasEntity(entity));
        }

        [Test]
        public void DisposeEntityCallsDispose()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            HumbleEntity entity = entityManager.CreateAndRegisterEntity();

            bool wasDisposed = false;
            entity.OnDisposed += () =>
            {
                wasDisposed = true;
            };

            entityManager.DisposeEntity(entity);

            Assert.IsTrue(wasDisposed);
        }

        [Test]
        public void DisposeEntityThrowsExceptionWhenNotFound()
        {
            HumbleEntityManager entityManager = CreateHumbleEntityManager();
            HumbleEntity entity = entityManager.CreateAndRegisterEntity();
            entityManager.UnRegister(entity);

            try
            {
                entityManager.DisposeEntity(entity);
                Assert.Fail();
            }
            catch (EntityNotFoundException exception)
            {
                Assert.AreEqual(exception.Entity, entity);
                Assert.IsFalse(string.IsNullOrEmpty(exception.Message));
            }
        }

        private HumbleEntityManager CreateHumbleEntityManager()
        {
            GameObject gameObject = new GameObject($"{nameof(HumbleEntityManager)}", typeof(HumbleEntityManager));
            return gameObject.GetComponent<HumbleEntityManager>();
        }
    }
}
