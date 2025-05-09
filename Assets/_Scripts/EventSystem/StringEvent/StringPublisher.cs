using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{

    [CreateAssetMenu(fileName = "New StringPublisher", menuName = "Scriptable Objects/Event System/StringPublisher")]
    public class StringPublisher : ScriptableObject
    {
        public UnityAction<string> OnEventRaise;

        public void ListenEvent(UnityAction<string> Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction<string> Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent(string val)
        {
            OnEventRaise?.Invoke(val);
        }
    }
}
