using UnityEngine;
using UnityEngine.SceneManagement;

namespace Toolbox
{
    public static class UnityClassesExtensions
    {
        /// <summary>
        /// Gets first found component from root objects on scene.
        /// Recomended to search only singletones that way.
        /// </summary>
        public static T GetRootComponent<T>(this Scene scene) where T : MonoBehaviour
        {
            if (scene.isLoaded)
            {
                T o;
                // search for active objects
                T[] objects = GameObject.FindObjectsOfType<T>();
                for (int i = 0; i < objects.Length; i++)
                {
                    o = objects[i];
                    if (o.gameObject.scene == scene)
                        return o;
                }

                // if root wasn't found try to get hidden objects instead
                objects = Resources.FindObjectsOfTypeAll<T>();
                for (int i = 0; i < objects.Length; i++)
                {
                    o = objects[i];
                    if (o.gameObject.scene == scene)
                        return o;
                }
            }

            return null;
        }

        /// <summary>
        /// RectTransform extension.
        /// </summary>
        public static void FillParent(this Transform transform)
        {
            RectTransform rt = transform as RectTransform;
            if(rt == null) throw new System.NullReferenceException();

            if (rt != null)
            {
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// RectTransform extension.
        /// </summary>
        public static void FillParent(this RectTransform rt)
        {
            if(rt == null) throw new System.NullReferenceException();

            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.localScale = Vector3.one;
        }
    }
}