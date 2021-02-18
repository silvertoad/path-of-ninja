using UnityEngine;
using UnityEngine.Events;

namespace PathToNinja.Model
{
    [CreateAssetMenu(fileName = "Event", menuName = "Binds/Events/Void", order = 1)]
    public class EventBind : ScriptableObject
    {
        public UnityEvent Event;
    }
}