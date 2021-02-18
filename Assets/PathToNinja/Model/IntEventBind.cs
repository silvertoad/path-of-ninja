using System;
using UnityEngine;
using UnityEngine.Events;

namespace PathToNinja.Model
{
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Binds/Events/Int", order = 1)]
    public class IntEventBind : ScriptableObject
    {
        public IntEvent Event;
    }

    [Serializable]
    public class IntEvent : UnityEvent<int>
    {
    }
}