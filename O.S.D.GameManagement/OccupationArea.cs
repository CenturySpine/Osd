using System.Windows;
using System.Windows.Media;
using O.S.D.Ui.Common;

namespace O.S.D.GameManagement
{
    public class OccupationArea :NotifierBase
    {
        private double _occupation;
        public Point StartPoint { get; set; }

        public double Occupation
        {
            get { return _occupation; }
            set { _occupation = value;Notify(); }
        }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Color Color { get; set; }

        public override string ToString()
        {
            return $"{nameof(StartPoint)}: {StartPoint}";
        }
    }
}