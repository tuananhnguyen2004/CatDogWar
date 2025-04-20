using SOEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class FloatEventListener : MonoBehaviour
    {
        [SerializeField] private FloatPublisher publisher;
        [SerializeField] private UnityEvent<float> responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond(float val)
        {
            responses?.Invoke(val);
        }
    }
}
