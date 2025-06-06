using System.Collections;
using UnityEngine;

namespace Extension {
    public static class IEnumeratorExtension {
        public static void Shuffle<T>(this IList target) {
            int count = target.Count;
            while (count > 1) {
                count--;
                int idx = Random.Range(0, count);
                (target[count], target[idx]) =
                    (target[idx], target[count]);
            }
        }
    }
}