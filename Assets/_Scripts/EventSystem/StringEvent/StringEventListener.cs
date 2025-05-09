using SOEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class StringEventListener : MonoBehaviour
    {
        [SerializeField] private StringPublisher publisher;
        [SerializeField] private UnityEvent<string> responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond(string val)
        {
            responses?.Invoke(val);
        }
    }
}
