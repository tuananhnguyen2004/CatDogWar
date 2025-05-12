using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using SOEventSystem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerTurn opponentTurn;
    [SerializeField] private VoidPublisher onAttacking;
    [SerializeField] private GameObject attackEffect;

    [Header("Animation Stats")]
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform inPosition;
    [SerializeField] private Vector3 handScale;
    private Vector2 initialBodyPosition;
    private Vector2 initialHandPosition;

    [Header("Player Object")]
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hand;

    private void Awake()
    {
        initialBodyPosition = body.transform.position;
        initialHandPosition = hand.transform.position;
    }

    public void AnimateOut()
    {
        body.transform.DOMove(initialBodyPosition, moveDuration);
    }

    public void AnimateIn()
    {
        body.transform.DOMove(inPosition.position, moveDuration);
    }

    public void AnimateBody(PlayerTurn playerTurn)
    {
        if (this.opponentTurn != playerTurn)
        {
            AnimateOut();
        }
        else
        {
            AnimateIn();
        }
    }

    public void AnimateAttack(Vector2 localPoint)
    {
        if (this.opponentTurn != GameManager.Instance.CurrentTurn) return;

        GameManager.Instance.IsAttacking = true;

        Vector2Int gridIndex = GameManager.Instance.CurrentGrid.GetGridCellIndex(localPoint);
        GridCell gridCell = GameManager.Instance.CurrentGrid.GetGridCellByIndex(gridIndex);

        if (gridCell.IsAttacked)
        {
            GameManager.Instance.IsAttacking = false;
            return;
        }

        AudioManager.Instance.PlaySoundFX("BeginAttack");

        hand.transform.DOScale(handScale, moveDuration / 2f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    hand.transform.DOScale(Vector3.one, moveDuration / 2f)
                        .SetEase(Ease.InOutSine);
                });

        hand.transform.DOMove(gridCell.Position, moveDuration)
            .OnComplete(async () =>
            {
                // Attack grid cell
                onAttacking.RaiseEvent();
                bool isHit = gridCell.GetAttacked();
                Instantiate(attackEffect, gridCell.Position, Quaternion.identity);

                if (isHit)
                    AudioManager.Instance.PlaySoundFX("ImpactHit");
                else
                    AudioManager.Instance.PlaySoundFX("ImpactMiss");

                await Task.Delay(100);

                hand.transform.DOMove(initialHandPosition, moveDuration)
                    .OnComplete(() =>
                    {
                        GameManager.Instance.IsAttacking = false;
                        if (isHit) return;
                        GameManager.Instance.SwitchTurn();
                    });
            });
    }
}
