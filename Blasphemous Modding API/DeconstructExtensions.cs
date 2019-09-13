using System.Collections.Generic;
using JetBrains.Annotations;

namespace Modding
{
    [PublicAPI]
    public static class DeconstructExtensions
    {
        public static void Deconstruct<TKey, TVal>(this KeyValuePair<TKey, TVal> self, out TKey key, out TVal val)
        {
            key = self.Key;
            val = self.Value;
        }
    }
}
