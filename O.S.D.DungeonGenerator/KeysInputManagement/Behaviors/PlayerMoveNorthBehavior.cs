using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.VisualManagement;

namespace O.S.D.DungeonGenerator.KeysInputManagement.Behaviors
{
    internal class PlayerMoveNorthBehavior : InputKeyBehavior
    {
        internal override void Execute(Generator gen, AreaDrawer drawer, Player player)
        {
            var nextpos = player.YPos - 1;
            if (gen.CanMove(player.XPos, nextpos))
            {
                player.YPos = nextpos;
                drawer.MovePlayer(player);
            }
        }
    }
}