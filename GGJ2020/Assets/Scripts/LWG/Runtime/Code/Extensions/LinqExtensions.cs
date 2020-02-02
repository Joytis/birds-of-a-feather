using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LWG {
    public static class LinqExtensions {
        public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> container, Predicate<T> predicate) {
            var success = container.Where(x => predicate(x));
            var failure = container.Where(x => !predicate(x));
            return (success, failure);
        }

        public static IEnumerable<(int, T)> ZipIndex<T>(this IEnumerable<T> container) {
            int index = 0;
            foreach(var item in container) {
                yield return (index, item);
                index++;
            }
        }
    }
}