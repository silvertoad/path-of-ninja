using System;
using UnityEngine;
using UnityEngine.Events;

namespace PathToNinja.Model
{
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Binds/Events/Bool", order = 1)]
    public class BoolEventBind : ScriptableObject
    {
        public BoolEvent Event;
    }
    
    [Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }
}