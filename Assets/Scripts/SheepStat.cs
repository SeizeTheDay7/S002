using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SheepStat : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI text_score;
    [SerializeField] Transform exp_bar;
    [SerializeField] GameObject LevelUpPanel;

    [Header("Sheep Stats")]
    public float speed_normal_base = 20f;
    public float speed_eating = 0.1f;
    public float eating_delay_base = 0.5f;
    [SerializeField] float moveSpeedLvlMultiplier = 0.5f;
    [SerializeField] float eatingDelayLvlMultiplier = 0.05f;
    [HideInInspector] public float speed_normal;
    [HideInInspector] public float eating_delay;
    [Header("Level")]
    [SerializeField] int exp_required_base = 5;
    [SerializeField] int exp_required_multiplier = 3;
    [SerializeField] TextMeshProUGUI valueUI_leg;
    [SerializeField] TextMeshProUGUI valueUI_teeth;
    [SerializeField] TextMeshProUGUI valueUI_bleat;
    [SerializeField] TextMeshProUGUI valueUI_fur;


    private int score = 0;
    private int exp = 0;
    private int exp_required = 5;
    private int lvl_sheep = 0;
    private Dictionary<string, int> lvl_components = new Dictionary<string, int>
    {
        { "leg", 1 },
        { "teeth", 1 },
        { "bleat", 1 },
        { "fur", 1 }
    };

    void Awake()
    {
        Reset();
    }

    public void GainEXP()
    {
        score += 1;
        exp += 1;
        UpdateUI_ScoreExp();
        TryLevelUp();
    }

    private void UpdateUI_ScoreExp()
    {
        text_score.text = "Score " + score;
        // exp_bar.localScale = new Vector3((float)exp / exp_required, 1, 1);
    }

    private void TryLevelUp()
    {
        if (exp < exp_required) return;
        GameManager.Instance.PauseGame();

        LevelUpPanel.SetActive(true);

        lvl_sheep += 1;
        exp = 0;
        exp_required = exp_required_base + lvl_sheep * exp_required_multiplier;
        UpdateUI_ScoreExp();
    }

    // Called by buttons on the level up panel
    public void ComponentLevelUp(string component)
    {
        lvl_components[component] += 1;
        UpdateStat();
        GameManager.Instance.ResumeGame();
        LevelUpPanel.SetActive(false);
    }

    private void UpdateStat()
    {
        speed_normal = speed_normal_base + moveSpeedLvlMultiplier * (lvl_components["leg"] - 1);
        eating_delay = eating_delay_base - eatingDelayLvlMultiplier * (lvl_components["teeth"] - 1);
        print("eating_dealy :" + eating_delay);

        UpdateUI_Lvl();
    }

    private void UpdateUI_Lvl()
    {
        valueUI_leg.text = lvl_components["leg"].ToString();
        valueUI_teeth.text = lvl_components["teeth"].ToString();
        valueUI_bleat.text = lvl_components["bleat"].ToString();
        valueUI_fur.text = lvl_components["fur"].ToString();
    }

    public void Reset()
    {
        score = 0;
        exp = 0;
        exp_required = exp_required_base;
        UpdateUI_ScoreExp();

        lvl_sheep = 0;
        foreach (var key in lvl_components.Keys.ToList()) { lvl_components[key] = 1; }
        UpdateUI_Lvl();

        speed_normal = speed_normal_base;
        eating_delay = eating_delay_base;
    }
}