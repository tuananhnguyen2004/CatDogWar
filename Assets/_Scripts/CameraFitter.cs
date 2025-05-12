using UnityEngine;

public class CameraFitter : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Screen Resolution: " + Screen.width + " " + Screen.height);
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / 2f;
        transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, transform.position.z);
    }
}
