using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiddleMast.GameplayFramework
{
    [DisallowMultipleComponent]
    public abstract class System : MonoBehaviour
    {
        public enum SetupTimings
        {
            Awake,
            Start,
            Custom,
        }

        [SerializeField] private SetupTimings _setupTiming = SetupTimings.Start;
        [SerializeField] private List<Manager> _managers = new List<Manager>();

        public SetupTimings SetupTiming
        {
            get => _setupTiming;
            set => _setupTiming = value;
        }

        public TManager GetManager<TManager>()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                Manager manager = _managers[i];

                if (manager is TManager casted)
                {
                    return casted;
                }
            }

            return default;
        }

        public bool HasManager<TManager>()
        {
            return _managers.Any(m=> m is TManager);
        }

        public bool HasManager(Manager manager)
        {
            return _managers.Contains(manager);
        }

        public void RegisterManager(Manager manager)
        {
            _managers.Add(manager);
        }

        public void UnRegisterManager(Manager manager)
        {
            _managers.Remove(manager);
        }

        public virtual void Setup()
        {
            SetupManagers();
        }

        public virtual void Tick(float deltaTime)
        {
            TickManagers(deltaTime);
        }

        public virtual void Dispose()
        {
            DisposeManagers();
        }

        protected virtual void SetupManagers()
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                Manager manager = _managers[i];
                manager.Setup();
            }
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
            if (SetupTiming == SetupTimings.Start)
            {
                Setup();
            }
        }

        private void Awake()
        {
            if (SetupTiming == SetupTimings.Awake)
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
            Tick(Time.deltaTime);
        }
    }
}
