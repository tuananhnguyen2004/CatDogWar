using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    [CreateAssetMenu(fileName = "New BoolPublisher", menuName = "Scriptable Objects/Event System/BoolPublisher")]
    public class BoolPublisher : ScriptableObject
    {
        public UnityAction<bool> OnEventRaise;

        public void ListenEvent(UnityAction<bool> Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction<bool> Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent(bool val)
        {
            OnEventRaise?.Invoke(val);
        }
    }
}
