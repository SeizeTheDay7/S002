using UnityEngine;

public class Manual : MonoBehaviour
{
    bool doneKeyboard;
    bool doneMouse;
    [SerializeField] bool debugMode = true;
    [SerializeField] GameObject keyboardSign;
    [SerializeField] GameObject mouseSign;
    [SerializeField] GameObject sheep;
    [SerializeField] GameObject score_UI;
    [SerializeField] GameObject shield_UI;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject PressEnterText;

    void Update()
    {
        if (!doneKeyboard && (Input.GetKeyDown(KeyCode.UpArrow) ||
                              Input.GetKeyDown(KeyCode.DownArrow) ||
                              Input.GetKeyDown(KeyCode.LeftArrow) ||
                              Input.GetKeyDown(KeyCode.RightArrow)))
        {
            doneKeyboard = true;
            keyboardSign.SetActive(true);
        }
        if (!doneMouse && Input.GetMouseButtonDown(0))
        {
            doneMouse = true;
            mouseSign.SetActive(true);
        }
        if (doneMouse && doneKeyboard && !PressEnterText.activeInHierarchy)
        {
            PressEnterText.SetActive(true);
        }
        if ((doneKeyboard && doneMouse && Input.GetKeyDown(KeyCode.Return)) || debugMode)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        sheep.SetActive(true);
        score_UI.SetActive(true);
        shield_UI.SetActive(true);
        gameManager.SetActive(true);
        gameObject.SetActive(false);
    }
}
