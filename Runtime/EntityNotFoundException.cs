using System;

namespace MiddleMast.GameplayFramework
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Entity entity)
        {
            Entity = entity;
        }

        public Entity Entity { get; }
        public override string Message => $"Entity '{(Entity == null ? "NULL" : Entity.name)}' was not found.";
    }
}
