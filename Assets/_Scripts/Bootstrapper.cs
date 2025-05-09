using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async Task InititializeScenes()
    {
        SceneManager.LoadScene("Bootstrapper", LoadSceneMode.Single);
        await SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }
}
