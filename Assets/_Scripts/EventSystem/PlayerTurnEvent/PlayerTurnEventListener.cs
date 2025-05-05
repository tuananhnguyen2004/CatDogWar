using SOEventSystem;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTurnEventListener : MonoBehaviour
{
    [SerializeField] private PlayerTurnPublisher publisher;
    [SerializeField] private UnityEvent<PlayerTurn> responses;

    private void OnEnable()
    {
        publisher.ListenEvent(Respond);
    }

    private void OnDisable()
    {
        publisher.UnlistenEvent(Respond);
    }

    private void Respond(PlayerTurn val)
    {
        responses?.Invoke(val);
    }
}
