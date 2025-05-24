using UnityEngine;

public class Menu : MonoBehaviour
{
    GameObject menu;
    [SerializeField] GameObject init;
    [SerializeField] GameObject mode;
    [SerializeField] GameObject manual;
    [SerializeField] GameObject Settings;

    void Start() { menu = gameObject; }

    public void StartButon()
    {
        init.SetActive(false);
        mode.SetActive(true);
    }

    public void NormalModeButton()
    {
        mode.SetActive(false);
        manual.SetActive(true);
    }

    public void HardModeButton()
    {
        mode.SetActive(false);
        GameManager.Instance.EnterHardMode();
        manual.GetComponent<Manual>().StartGame();
    }

    public void SettingsButton()
    {
        init.SetActive(false);
        Settings.SetActive(true);
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
