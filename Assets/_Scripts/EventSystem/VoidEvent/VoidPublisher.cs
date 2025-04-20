using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    [CreateAssetMenu(fileName = "New VoidPublisher", menuName = "Scriptable Objects/Event System/VoidPublisher")]
    public class VoidPublisher : ScriptableObject
    {
        public UnityAction OnEventRaise;

        public void ListenEvent(UnityAction Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent()
        {
            OnEventRaise?.Invoke();
        }
    }
}
