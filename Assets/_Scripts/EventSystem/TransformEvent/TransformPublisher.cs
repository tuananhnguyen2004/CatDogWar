using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Transform Publisher", menuName = "Scriptable Objects/Event System/TransdormPublisher")]
public class TransformPublisher : ScriptableObject
{
    public UnityAction<Transform> OnEventRaise;

    public void ListenEvent(UnityAction<Transform> Action)
    {
        OnEventRaise += Action;
    }

    public void UnlistenEvent(UnityAction<Transform> Action)
    {
        OnEventRaise -= Action;
    }

    public void RaiseEvent(Transform val)
    {
        OnEventRaise?.Invoke(val);
    }
}
