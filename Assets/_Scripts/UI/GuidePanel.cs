using UnityEngine;
using DG.Tweening;

public class GuidePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float animateSpeed;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, animateSpeed);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, animateSpeed).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
        });
    }
}
