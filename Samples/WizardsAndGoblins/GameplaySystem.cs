using MiddleMast.GameplayFramework.Samples.WizardsAndGoblins.Spells;
using MiddleMast.GameplayFramework.Samples.WizardsAndGoblins.Wizards;

namespace MiddleMast.GameplayFramework.Samples.WizardsAndGoblins
{
    internal class GameplaySystem : System
    {
        protected override void SetupManagers()
        {
            base.SetupManagers();

            SpellManager spellManager = GetManager<SpellManager>();

            WizardManager wizardManager = GetManager<WizardManager>();
            wizardManager.Setup(spellManager);
        }
    }
}
