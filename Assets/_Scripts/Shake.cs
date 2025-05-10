using DG.Tweening;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90.0f;
    [SerializeField] private float strength = 1.0f;
    [SerializeField] private bool snapping = false;
    [SerializeField] private bool fadeOut = true;

    public void DoShake()
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
    }
}
