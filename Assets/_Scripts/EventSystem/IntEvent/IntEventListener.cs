using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class IntEventListener : MonoBehaviour
    {
        [SerializeField] private IntPublisher publisher;
        [SerializeField] private UnityEvent<int> responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond(int val)
        {
            responses?.Invoke(val);
        }
    }
}
