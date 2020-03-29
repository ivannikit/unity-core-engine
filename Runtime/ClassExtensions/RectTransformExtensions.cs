#if UNITY_2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox
{
    public static class RectTransformExtensions
    {
        public static Rect ToWorldSpace(this RectTransform rt)
        {
            Vector2 size = Vector2.Scale(rt.rect.size, rt.lossyScale);

            Vector2 position = rt.position;
            Vector2 pivotDelta = rt.pivot * size;
            position -= pivotDelta;

            return new Rect(position, size);
        }
    }
}
#endif
