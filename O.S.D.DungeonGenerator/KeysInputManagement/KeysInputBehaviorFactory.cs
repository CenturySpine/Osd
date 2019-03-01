using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.KeysInputManagement.Behaviors;
using O.S.D.DungeonGenerator.VisualManagement;
using System.Collections.Generic;
using System.Windows.Input;

namespace O.S.D.DungeonGenerator.KeysInputManagement
{
    internal class KeysInputBehaviorFactory
    {
        private readonly Generator _gen;
        private readonly AreaDrawer _drawer;
        private Dictionary<Key, InputKeyBehavior> _keyToBehaviorsMappings;

        public KeysInputBehaviorFactory(Generator gen, AreaDrawer drawer)
        {
            _gen = gen;
            _drawer = drawer;
            InitializeMappings();
        }

        private void InitializeMappings()
        {
            _keyToBehaviorsMappings = new Dictionary<Key, InputKeyBehavior>
            {
                {Key.Up,new PlayerMoveNorthBehavior() },
                {Key.Down,new PlayerMoveSouthBehavior()},
                {Key.Left,new PlayerMoveWestBehavior()},
                {Key.Right,new PlayerMoveEastBehavior() },
                {Key.LeftCtrl,new PlayerStanceChangeBehavior()},
            };
        }

        public bool IsManaged(Key eKey, Player player)
        {
            InputKeyBehavior beh;
            if (_keyToBehaviorsMappings.TryGetValue(eKey, out beh))
            {
                _drawer.Focus();
                beh.Execute(_gen, _drawer, player);
                return true;
            }
            return false;
        }
    }
}