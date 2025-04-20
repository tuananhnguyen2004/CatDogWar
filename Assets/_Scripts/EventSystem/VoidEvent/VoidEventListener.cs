using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private VoidPublisher publisher;
        [SerializeField] private UnityEvent responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond()
        {
            responses?.Invoke();
        }
    }
}
