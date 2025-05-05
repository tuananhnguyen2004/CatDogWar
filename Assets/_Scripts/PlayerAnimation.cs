using UnityEngine;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerTurn playerTurn;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveDuration;

    public void AnimateIn(PlayerTurn playerTurn)
    {
        if (this.playerTurn != playerTurn) return;

        transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
            .OnComplete(() =>
            {
                // Can attack
            });
    }

    public void AnimateOut(PlayerTurn playerTurn)
    {
        if (this.playerTurn == playerTurn) return;

        transform.DOMoveY(transform.position.y - moveDistance, moveDuration);
    }
}
