using WizardsAndGoblins.Spells.Manager;
using WizardsAndGoblins.Wizards.Manager;

namespace WizardsAndGoblins.System
{
    internal class GameplaySystem : MiddleMast.GameplayFramework.System
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
