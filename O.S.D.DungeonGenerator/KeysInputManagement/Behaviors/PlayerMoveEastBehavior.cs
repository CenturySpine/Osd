using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.VisualManagement;

namespace O.S.D.DungeonGenerator.KeysInputManagement.Behaviors
{
    internal class PlayerMoveEastBehavior : InputKeyBehavior
    {
        internal override void Execute(Generator gen, AreaDrawer drawer, Player player)
        {
            var nextpos = player.XPos + 1; ;
            if (gen.CanMove(nextpos, player.YPos))
            {
                player.XPos = nextpos;
                drawer.MovePlayer(player);
            }
        }
    }
}