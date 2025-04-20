using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    [CreateAssetMenu(fileName = "New IntPublisher", menuName = "Scriptable Objects/Event System/IntPublisher")]
    public class IntPublisher : ScriptableObject
    {
        public UnityAction<int> OnEventRaise;

        public void ListenEvent(UnityAction<int> Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction<int> Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent(int val)
        {
            OnEventRaise?.Invoke(val);
        }
    }
}
