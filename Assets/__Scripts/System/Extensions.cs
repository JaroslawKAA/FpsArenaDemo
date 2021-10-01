using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace __Scripts
{
    public static class Extensions
    {
        public static bool IsAnimatorState(this Animator animator, string name, int layerId = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerId).IsName(name);
        }

        public static Dictionary<string, float> GetClipsLengths(this Animator animator)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

            foreach (var clip in clips)
            {
                result.Add(clip.name, clip.length);
            }

            return result;
        }

        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            return collection.Shuffle().First();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            return collection
                .OrderBy(o => UnityEngine.Random.value);
        }
    }
}