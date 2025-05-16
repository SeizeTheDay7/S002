using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject restart;
    [SerializeField] private SheepStat sheepStat;
    [SerializeField] private SheepMove sheepMove;
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

    public void RestartGame()
    {
        ResumeGame();
        restart.SetActive(false);
        sheepStat.Reset();
        sheepMove.Reset();
        wolfManager.Reset();
        grassManager.Reset();
    }
}