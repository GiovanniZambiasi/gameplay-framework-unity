using System.Linq;
using UnityEngine;

namespace MiddleMast.GameplayFramework.Tests
{
    public class HumbleEntityManager : EntityManager<HumbleEntity>
    {
        public override void Register(HumbleEntity entity)
        {
            entity.Setup();
            base.Register(entity);
        }

        public HumbleEntity CreateAndRegisterEntity()
        {
            GameObject gameObject = new GameObject($"{nameof(HumbleEntity)}", typeof(HumbleEntity));
            HumbleEntity entity = gameObject.GetComponent<HumbleEntity>();

            Register(entity);

            return entity;
        }

        public bool HasEntity(HumbleEntity entity)
        {
            return Entities.Contains(entity);
        }
    }
}
