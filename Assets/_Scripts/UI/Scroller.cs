using SOEventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scroller : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 scrollDirection;
    [SerializeField] private StringPublisher onEnterGameScene;
    private RawImage image;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySoundFX("ButtonClick");
        onEnterGameScene.RaiseEvent("Gameplay");
    }

    public void Log()
    {
        Debug.Log("Scroller");
    }

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + scrollDirection * speed * Time.deltaTime, image.uvRect.size);
    }
}
