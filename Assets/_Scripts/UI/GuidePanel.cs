using UnityEngine;
using DG.Tweening;

public class GuidePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    [SerializeField] protected float animateSpeed;

    virtual protected void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
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
