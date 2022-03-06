using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace MiddleMast.GameplayFramework.Tests
{
    public class SystemTests
    {
        [Test]
        public void SetsUpManagersInOrder()
        {
            System system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            List<Manager> initializationOrder = new List<Manager>();

            HumbleManager earlyManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            earlyManager.OnSetup += manager => initializationOrder.Add(manager);
            system.RegisterManager(earlyManager);

            HumbleManager middleManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            middleManager.OnSetup += manager => initializationOrder.Add(manager);
            system.RegisterManager(middleManager);

            HumbleManager lateManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            lateManager.OnSetup += manager => initializationOrder.Add(manager);
            system.RegisterManager(lateManager);

            List<Manager> expectedInitializationOrder = new List<Manager>
            {
                earlyManager,
                middleManager,
                lateManager,
            };

            system.Setup();

            Assert.IsTrue(initializationOrder.SequenceEqual(expectedInitializationOrder));
        }

        [Test]
        public void DisposesManagersInReverseOrder()
        {
            System system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            List<Manager> disposalOrder = new List<Manager>();

            HumbleManager earlyManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            earlyManager.OnDispose += manager => disposalOrder.Add(manager);
            system.RegisterManager(earlyManager);

            HumbleManager middleManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            middleManager.OnDispose += manager => disposalOrder.Add(manager);
            system.RegisterManager(middleManager);

            HumbleManager lateManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            lateManager.OnDispose += manager => disposalOrder.Add(manager);
            system.RegisterManager(lateManager);

            List<Manager> expectedDisposalOrder = new List<Manager>
            {
                lateManager,
                middleManager,
                earlyManager,
            };

            system.Setup();
            system.Dispose();

            Assert.IsTrue(disposalOrder.SequenceEqual(expectedDisposalOrder));
        }

        [Test]
        public void TicksManagersInOrder()
        {
            System system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            List<Manager> tickOrder = new List<Manager>();

            HumbleManager earlyManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            earlyManager.OnTick += manager => tickOrder.Add(manager);
            system.RegisterManager(earlyManager);

            HumbleManager middleManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            middleManager.OnTick += manager => tickOrder.Add(manager);
            system.RegisterManager(middleManager);

            HumbleManager lateManager = SystemTestUtilities.CreateManager<HumbleManager>(system);
            lateManager.OnTick += manager => tickOrder.Add(manager);
            system.RegisterManager(lateManager);

            List<Manager> expectedTickOrder = new List<Manager>
            {
                earlyManager,
                middleManager,
                lateManager,
            };

            system.Setup();
            system.Tick(.5f);

            Assert.IsTrue(tickOrder.SequenceEqual(expectedTickOrder));
        }

        [Test]
        public void RegistersManager()
        {
            System system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());

            Manager manager = SystemTestUtilities.CreateManager(system, typeof(HumbleManager));

            system.RegisterManager(manager);

            Assert.IsTrue(system.HasManager(manager));
        }

        [Test]
        public void UnRegistersManager()
        {
            System system = SystemTestUtilities.CreateSystem<HumbleSystem>(new [] { typeof(HumbleManager) });

            Manager manager = system.GetManager<HumbleManager>();

            Assert.IsNotNull(manager);

            system.UnRegisterManager(manager);

            Assert.IsFalse(system.HasManager(manager));
        }

        [Test]
        public void SetupsUpDuringAwake()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>(), System.SetupTimings.Awake);

            Assert.IsTrue(system.IsSetup);
        }

        [Test]
        public void GetManagerSimple()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(new[]
            {
                typeof(AnotherHumbleManager),
                typeof(HumbleManager),
            });
            HumbleManager humbleManager = system.GetManager<HumbleManager>();

            Assert.IsNotNull(humbleManager);
        }

        [Test]
        public void GetManagerReturnsNullWhenNotFound()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>());
            HumbleManager humbleManager = system.GetManager<HumbleManager>();

            Assert.IsNull(humbleManager);
        }

        [Test]
        public void HasManagerSimple()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(new[]
            {
                typeof(AnotherHumbleManager),
                typeof(HumbleManager),
            });

            Assert.IsTrue(system.HasManager<HumbleManager>());
        }

        [Test]
        public void HasManagerReturnsFalse()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(new[]
            {
                typeof(AnotherHumbleManager),
            });

            Assert.IsFalse(system.HasManager<HumbleManager>());
        }

        [UnityTest]
        public IEnumerator SetupsUpDuringStart()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>(), System.SetupTimings.Start);

            Assert.IsFalse(system.IsSetup);

            yield return new WaitForEndOfFrame();

            Assert.IsTrue(system.IsSetup);
        }

        [UnityTest]
        public IEnumerator SetsUpWithCustom()
        {
            HumbleSystem system = SystemTestUtilities.CreateSystem<HumbleSystem>(Array.Empty<Type>(), System.SetupTimings.Custom);

            Assert.IsFalse(system.IsSetup);

            yield return null;

            Assert.IsFalse(system.IsSetup);

            system.Setup();

            Assert.IsTrue(system.IsSetup);
        }
    }
}
