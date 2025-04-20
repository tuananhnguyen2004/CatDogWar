using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    [CreateAssetMenu(fileName = "New FloatPublisher", menuName = "Scriptable Objects/Event System/FloatPublisher")]
    public class FloatPublisher : ScriptableObject
    {
        public UnityAction<float> OnEventRaise;

        public void ListenEvent(UnityAction<float> Action)
        {
            OnEventRaise += Action;
        }

        public void UnlistenEvent(UnityAction<float> Action)
        {
            OnEventRaise -= Action;
        }

        public void RaiseEvent(float val)
        {
            OnEventRaise?.Invoke(val);
        }
    }
}
