using UnityEngine;

namespace Toolbox
{
    public static class UnityObjectExtensions
    {
        public static void SafeDestroy(this Object o)
        {
            if(o == null) throw new System.NullReferenceException();

    #if UNITY_EDITOR
            if (Application.isPlaying)
                Object.Destroy(o);
            else
                Object.DestroyImmediate(o);
    #else
            Object.Destroy(o);
    #endif
        }
    }
}
