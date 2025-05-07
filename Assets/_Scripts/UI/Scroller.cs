using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scroller : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Log();
    }
    [SerializeField] private float speed;
    [SerializeField] private Vector2 scrollDirection;
    private RawImage image;

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
