using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.VisualManagement;

namespace O.S.D.DungeonGenerator.KeysInputManagement.Behaviors
{
    internal abstract class InputKeyBehavior
    {
        internal abstract void Execute(Generator gen, AreaDrawer drawer, Player player);
    }
}