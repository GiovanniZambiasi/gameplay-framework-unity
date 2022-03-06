using System.Collections.Generic;

namespace MiddleMast.GameplayFramework
{
    /// <summary>
    /// Helper class that includes some <see cref="Entity"/> management boilerplate
    /// </summary>
    public class EntityManager<TEntity> : Manager where TEntity : Entity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();

        protected IReadOnlyList<TEntity> Entities => _entities;

        /// <summary>
        /// Adds the entity to the tick list
        /// </summary>
        public virtual void Register(TEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Removes the entity from the tick list
        /// <returns>true if entity found in list</returns>
        /// </summary>
        public virtual bool UnRegister(TEntity entity)
        {
            return _entities.Remove(entity);
        }

        /// <summary>
        /// Removes the entity from the tick list, disposes and destroys it
        /// </summary>
        public virtual void DisposeEntity(TEntity entity)
        {
            if (UnRegister(entity))
            {
                entity.Dispose();
                Destroy(entity.gameObject);
            }
            else
            {
                throw new EntityNotFoundException(entity);
            }
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            TickEntities(deltaTime);
        }

        protected virtual void TickEntities(float deltaTime)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                TEntity entity = _entities[i];
                entity.Tick(deltaTime);
            }
        }
    }
}
