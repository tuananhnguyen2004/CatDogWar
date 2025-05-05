using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New PlayerTurnPublisher", menuName = "Scriptable Objects/Event System/PlayerTurnPublisher")]
public class PlayerTurnPublisher : ScriptableObject
{
    public UnityAction<PlayerTurn> OnEventRaise;

    public void ListenEvent(UnityAction<PlayerTurn> Action)
    {
        OnEventRaise += Action;
    }

    public void UnlistenEvent(UnityAction<PlayerTurn> Action)
    {
        OnEventRaise -= Action;
    }

    public void RaiseEvent(PlayerTurn val)
    {
        OnEventRaise?.Invoke(val);
    }
}
