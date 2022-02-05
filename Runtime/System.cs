using System.Collections.Generic;
using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    [DisallowMultipleComponent]
    public abstract class System : MonoBehaviour
    {
        private enum SetupTiming
        {
            Awake,
            Start,
            Custom,
        }

        [SerializeField] private SetupTiming _setupTiming = SetupTiming.Start;
        [SerializeField] private List<Manager> _managers = new List<Manager>();

        public void Setup()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                Manager manager = _managers[i];
                manager.Setup();
            }
        }

        public virtual void Tick()
        {
            float deltaTime = Time.deltaTime;

            TickManagers(deltaTime);
        }

        public virtual void Dispose()
        {
            DisposeManagers();
        }

        protected virtual void TickManagers(float deltaTime)
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                Manager manager = _managers[i];
                manager.Tick(deltaTime);
            }
        }

        protected virtual void DisposeManagers()
        {
            for (int i = _managers.Count - 1; i >= 0; i--)
            {
                Manager manager = _managers[i];
                manager.Dispose();
            }
        }

        private void Start()
        {
            if (_setupTiming == SetupTiming.Start)
            {
                Setup();
            }
        }

        private void Awake()
        {
            if (_setupTiming == SetupTiming.Awake)
            {
                Setup();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Update()
        {
            Tick();
        }
    }
}
