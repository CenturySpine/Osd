using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace O.S.D.GameManagement
{
    static class ColorHelper
    {
        private static Random _rnd;
        private static Dictionary<Color, bool> _dict;

        static ColorHelper()
        {
            var t = typeof(Colors);
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
            _dict = new Dictionary<Color, bool>();
            foreach (var pi in props)
            {
                Color c = (Color)pi.GetValue(null);
                if (!_dict.ContainsKey(c))
                    _dict.Add(c, false);
            }
            _rnd = new Random((int)(DateTime.Now.Millisecond / 0.2));
        }

        internal static Color get()
        {

            var free = _dict.Where(r => !r.Value).ToList();
            var index = _rnd.Next(0, free.Count);
            var color = free[index];

            _dict[color.Key] = true;

            return color.Key;
        }
    }
}