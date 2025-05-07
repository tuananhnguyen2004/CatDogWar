using UnityEngine;
using TMPro;
using DG.Tweening;

public class BounceTitle : MonoBehaviour
{
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleFactor;
    private void Start()
    {
        transform.DOScale(transform.localScale * scaleFactor, scaleSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
