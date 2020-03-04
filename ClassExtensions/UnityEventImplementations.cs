using System;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.Events
{
    [Serializable]
    public class StringUnityEvent : UnityEvent<String> { }

    [Serializable]
    public class SingleUnityEvent : UnityEvent<Single> { }

    [Serializable]
    public class Int32UnityEvent : UnityEvent<Int32> { }

    [Serializable]
    public class BooleanUnityEvent : UnityEvent<Boolean> { }

    [Serializable]
    public class SpriteUnityEvent : UnityEvent<Sprite> { }
}
