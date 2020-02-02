using System;
using System.Collections.Generic;
using System.Linq;

namespace LWG {

public static class EnumerableExtensions {
    // No Omnisharp. It can not be IEnumerable. 
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng) {
        T[] elements = source.ToArray();
        // Using Fishes-Yates Shuffle
        for (int i = elements.Length - 1; i >= 0; i--) {
            int swapIndex = rng.Next(i + 1);
            yield return elements[swapIndex];
            elements[swapIndex] = elements[i];
        }
    }
}
} // namespace LWG