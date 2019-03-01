using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.VisualManagement;

namespace O.S.D.DungeonGenerator.KeysInputManagement.Behaviors
{
    internal class PlayerStanceChangeBehavior : InputKeyBehavior
    {
        internal override void Execute(Generator gen, AreaDrawer drawer, Player player)
        {
            player.ChangeStance();
            drawer.ChangePlayerStance(player);
        }
    }
}