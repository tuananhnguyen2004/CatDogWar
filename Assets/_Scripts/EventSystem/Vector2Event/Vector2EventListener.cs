using SOEventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace SOEventSystem
{
    public class Vector2EventListener : MonoBehaviour
    {
        [SerializeField] private Vector2Publisher publisher;
        [SerializeField] private UnityEvent<Vector2> responses;

        private void OnEnable()
        {
            publisher.ListenEvent(Respond);
        }

        private void OnDisable()
        {
            publisher.UnlistenEvent(Respond);
        }

        private void Respond(Vector2 val)
        {
            responses?.Invoke(val);
        }
    }
}
