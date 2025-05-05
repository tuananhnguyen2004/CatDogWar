using UnityEngine;
using TMPro;

public class GameOverPanel : GuidePanel
{
    [SerializeField] private TMP_Text gameOverText;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Show(int playerNum)
    {
        gameOverText.text = "Player " + playerNum + " wins!";
        Show();
    }
}
