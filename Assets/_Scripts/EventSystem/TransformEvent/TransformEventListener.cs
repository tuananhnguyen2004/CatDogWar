using SOEventSystem;
using UnityEngine;
using UnityEngine.Events;

public class TransformEventListener : MonoBehaviour
{
    [SerializeField] private TransformPublisher publisher;
    [SerializeField] private UnityEvent<Transform> responses;

    private void OnEnable()
    {
        publisher.ListenEvent(Respond);
    }

    private void OnDisable()
    {
        publisher.UnlistenEvent(Respond);
    }

    private void Respond(Transform val)
    {
        responses?.Invoke(val);
    }
}
