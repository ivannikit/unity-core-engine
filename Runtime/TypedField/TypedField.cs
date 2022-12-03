using UnityEngine;

namespace TeamZero.Inspector
{
    [System.Serializable]
    [TypeRistrictionTarget("_unityObject")]
    public class TypedField
    {
        [SerializeField]
        private Object _unityObject;

        private object _systemObject;

        private object Value
        {
            get
            {
                if (_unityObject != null)
                    return _unityObject;
                else
                    return _systemObject;
            }
            set
            {
                if (value is Object)
                {
                    _unityObject = value as Object;
                    _systemObject = null;
                }
                else
                {
                    _systemObject = value;
                    _unityObject = null;
                }
            }
        }

        public T Get<T>() where T : class
        {
            object value = Value;
            if (value != null)
            {
                T result = (T)value;
                if (result == null)
                    Debug.LogErrorFormat("{0} excepted {1}", typeof(TypedField), typeof(T));

                return result;
            }
            else
                return null;
        }

        public void Set(object value)
        {
            Value = value;
        }
    }
}
