using System;
using UnityEngine;

namespace TeamZero
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Find and return RectTransform component
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mode"></param>
        /// <returns>RectTransform or null</returns>
        public static RectTransform GetRectTransform(this MonoBehaviour target, bool exception = false)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            RectTransform rt = target.transform as RectTransform;
            if ( rt == null && exception)
                throw new NullReferenceException("RectTransform");

            return rt;
        }

        /// <summary>
        /// Find and return RectTransform component. If RectTransform is null created new component.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>Always not null</returns>
        public static RectTransform GetOrCreateRectTransform(this MonoBehaviour target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            RectTransform rt = target.transform as RectTransform;
            if (rt == null )
            {
                rt = target.gameObject.AddComponent<RectTransform>();
            }

            return rt;
        }
    }
}
