using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject restart;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameEndSequence gameEndSequence;
    [SerializeField] private Sheep sheep;
    [SerializeField] private SheepStat sheepStat;
    [SerializeField] private SheepMove sheepMove;
    [SerializeField] private SheepBarrier sheepAbility;
    [SerializeField] private WolfManager wolfManager;
    [SerializeField] private GrassManager grassManager;
    [SerializeField] private ShotManager shotManager;
    SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.Instance;
    }

    public void PauseGame()
    {
        soundManager.PauseAllSound();
        soundManager.BGMAudioSource.volume = 0.5f;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        soundManager.ResumeAllSound();
        soundManager.BGMAudioSource.volume = 1f;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        soundManager.StopAllSound();
        PauseGame();
        soundManager.GameOverSfx();
        restart.SetActive(true);
    }

    public void EnterHardMode()
    {
        sheepStat.EnterHardMode();
        wolfManager.EnterHardMode();
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
        else if (score == 60)
        {
            shotManager.enabled = true;
        }
        else if (score == 75)
        {
            shotManager.reloadTime /= 2;
        }
        else if (score == 100)
        {
            EndGame();
        }
    }

    public void RestartGame()
    {
        soundManager.PlayBGM();
        restart.SetActive(false);
        sheepStat.Reset();
        sheepMove.Reset();
        sheepAbility.Reset();
        wolfManager.Reset();
        grassManager.Reset();
        shotManager.Reset();
        ResumeGame();
    }

    private void EndGame()
    {
        soundManager.StopBGM();
        soundManager.StopAllSound();
        inGameUI.SetActive(false);
        sheep.EndGame();
        sheepMove.EndGame();
        wolfManager.EndGame();
        shotManager.EndGame();
        grassManager.EndGame();
        gameEndSequence.PlayGameEndSequence();
    }
}