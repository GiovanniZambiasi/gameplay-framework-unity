using System.Collections.Generic;

namespace WizardsAndGoblins.Goblins.Manager
{
    internal class GoblinManager : MiddleMast.GameplayFramework.Manager
    {
        private List<Goblin> _goblins = new List<Goblin>();

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            for (int i = 0; i < _goblins.Count; i++)
            {
                Goblin goblin = _goblins[i];
                goblin.Tick(deltaTime);
            }
        }
    }
}
