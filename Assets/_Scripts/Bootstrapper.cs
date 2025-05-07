using UnityEngine;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void InititializeScenes()
    {
        //float ratio = 1284f / 2778f;
        //Screen.SetResolution((int)(Screen.height * ratio), Screen.height, FullScreenMode.FullScreenWindow);
    }
}
