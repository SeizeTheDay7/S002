using UnityEngine;

public class Menu : MonoBehaviour
{
    GameObject menu;
    [SerializeField] GameObject init;
    [SerializeField] GameObject mode;
    [SerializeField] GameObject manual;
    [SerializeField] GameObject credit;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject titleSheep;
    SoundManager soundManager;

    void Start()
    {
        menu = gameObject;
        soundManager = SoundManager.Instance;
    }

    public void StartButon()
    {
        soundManager.ClickSfx();
        init.SetActive(false);
        mode.SetActive(true);
    }

    public void NormalModeButton()
    {
        soundManager.ClickSfx();
        soundManager.StopBGM();
        mode.SetActive(false);
        titleSheep.SetActive(false);
        manual.SetActive(true);
    }

    public void HardModeButton()
    {
        soundManager.ClickSfx();
        mode.SetActive(false);
        titleSheep.SetActive(false);
        GameManager.Instance.EnterHardMode();
        manual.GetComponent<Manual>().StartGame();
    }

    public void CreditButton()
    {
        init.SetActive(false);
        credit.SetActive(true);
    }

    public void SettingsButton()
    {
        init.SetActive(false);
        settings.SetActive(true);
    }

    public void HomeButton(GameObject menufrom)
    {
        menufrom.SetActive(false);
        init.SetActive(true);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
