using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.ExtensionMethod
{
    public static class ExtensionMethod
    {
        public static void Shuffle<T>(this List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            Random random = new ();
            int n = list.Count;

            while (n > 1)
            {
                int k = random.Next(n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    
        public static List<T> Clone<T>(this IList<T> listToClone) where T: ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}