using SOEventSystem;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VoidPublisher onGameStart;
    [SerializeField] private float gameStartDelay;

    private void Start()
    {
        StartGame();
    }

    private async void StartGame()
    {
        await Task.Delay((int)(gameStartDelay * 1000));
        onGameStart.RaiseEvent();
    }
}
