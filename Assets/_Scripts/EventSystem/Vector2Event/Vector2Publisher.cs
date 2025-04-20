using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    [CreateAssetMenu(fileName = "New FloatPublisher", menuName = "Scriptable Objects/Event System/Vector2Publisher")]
    public class Vector2Publisher : ScriptableObject
    {
        public UnityAction<Vector2> OnEventRaise;

        public void ListenEvent(UnityAction<Vector2> Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction<Vector2> Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent(Vector2 val)
        {
            OnEventRaise?.Invoke(val);
        }
    }
}
