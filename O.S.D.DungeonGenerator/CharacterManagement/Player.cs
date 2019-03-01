using System.ComponentModel;

namespace O.S.D.DungeonGenerator.CharacterManagement
{
    internal class Player
    {
        public Player()
        {
            Stance = PlayerStance.Up;
            VisionRadius = 4;
            Velocity = 5;
        }

        public int XPos { get; set; }
        public int YPos { get; set; }

        public PlayerStance Stance { get; set; }

        public void ChangeStance()
        {
            if (Stance == PlayerStance.Crouch)
            {
                Stance = PlayerStance.Up;
                Velocity = 5;
            }
            else
            {
                Stance = PlayerStance.Crouch;
                Velocity = 2;
            }
        }

        internal int GetModifiedVelocity()
        {
            switch (Velocity)
            {
                case 1:
                    return 200;
                    break;
                case 2:
                    return 150;
                    break;
                case 5:
                    return 100;
                    break;
                case 10:
                    return 0;
                    break;
                default:
                    throw new InvalidEnumArgumentException("Velocity not in valid range values");
            }
        }

        public int Velocity { get; set; }

        public int VisionRadius { get; set; }
    }
}