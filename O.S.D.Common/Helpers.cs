using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O.S.D.Common
{
    public class Helpers
    {
        public static T Clamp<T>(T value, T minimum, T maximum)
            where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(minimum) < 0)
                result = minimum;
            if (value.CompareTo(maximum) > 0)
                result = maximum;
            return result;
        }
    }
}
