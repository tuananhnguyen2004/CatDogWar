using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Transform sceneTransitionSprite;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private Transform transitionTo;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = sceneTransitionSprite.position;
    }

    public void LoadScene(string sceneName)
    {
        sceneTransitionSprite.DOMove(transitionTo.position, transitionDuration)
            .OnComplete(async () =>
            {                
                await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                sceneTransitionSprite.DOMove(initialPosition, transitionDuration);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            });
    }

}
