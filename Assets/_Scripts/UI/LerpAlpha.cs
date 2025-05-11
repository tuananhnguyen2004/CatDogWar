using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LerpAlpha : MonoBehaviour
{
    [SerializeField] private float lerpDuration = 1.0f;
    [SerializeField] private PlayerTurn opponentTurn;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LerpCanvasAlpha(PlayerTurn turn)
    {
        if (turn != opponentTurn)
        {
            StartCoroutine(LerpAlphaCoroutine(0f));
            Debug.Log("LerpCanvasAlpha Out: " + turn);
        }
        else
        {
            Debug.Log("LerpCanvasAlpha In: " + turn);
            StartCoroutine(LerpAlphaCoroutine(1f));
        }
    }

    public IEnumerator LerpAlphaCoroutine(float targetAlpha)
    {
        float elapsedTime = 0f;
        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, elapsedTime);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
