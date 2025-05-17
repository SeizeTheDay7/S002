using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject restart;
    [SerializeField] private SheepStat sheepStat;
    [SerializeField] private SheepMove sheepMove;
    [SerializeField] private SheepBarrier sheepAbility;
    [SerializeField] private WolfManager wolfManager;
    [SerializeField] private GrassManager grassManager;

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        PauseGame();
        restart.SetActive(true);
    }

    public void BalanceGame(int score)
    {
        if (score == 15)
        {
            wolfManager.chaseDuration = 0.75f;
        }
        else if (score == 30)
        {
            wolfManager.wolfAmount = 2;
        }
        else if (score == 45)
        {
            wolfManager.wolfAmount = 3;
        }
    }

    public void RestartGame()
    {
        restart.SetActive(false);
        sheepStat.Reset();
        sheepMove.Reset();
        sheepAbility.Reset();
        wolfManager.Reset();
        grassManager.Reset();
        ResumeGame();
    }
}