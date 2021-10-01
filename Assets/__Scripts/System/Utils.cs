using System;
using System.Collections;
using UnityEngine;

namespace __Scripts
{
    public static class Utils
    {
        /// <summary>
        /// Coroutine for delaying action.
        /// </summary>
        /// <param name="delay">Time in seconds</param>
        /// <param name="callback">Action played after delayed time.</param>
        /// <returns></returns>
        public static IEnumerator Delay(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback();
        }
        
        public delegate IEnumerator Callback(float delay);
    }
}