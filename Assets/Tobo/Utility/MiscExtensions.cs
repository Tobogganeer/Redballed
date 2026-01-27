using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobo.Util
{
    public static class MiscExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        // Used for hashes across net clients and such. Taken from Mirror
        public static int GetStableHashCode(this string text)
        {
            unchecked
            {
                int hash = 23;
                foreach (char c in text)
                    hash = hash * 31 + c;
                return hash;
            }
        }
    }
}
