using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class BoolEventListener : MonoBehaviour
    {
        [SerializeField] private BoolPublisher publisher;
        [SerializeField] private UnityEvent<bool> responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond(bool val)
        {
            responses?.Invoke(val);
        }
    }
}
