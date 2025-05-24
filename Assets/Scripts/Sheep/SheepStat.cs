using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class SheepStat : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI text_score;
    [SerializeField] GameObject LevelUpPanel;
    [SerializeField] GameObject heart_prefab;
    [SerializeField] Transform HP_Parent;
    private List<GameObject> UI_Hearts = new List<GameObject>();

    [Header("Sheep Stats")]
    public int hp_base = 3;
    private int max_hp;
    public float speed_normal_base = 20f;
    public float speed_eating = 0.1f;
    public float eating_delay_base = 0.5f;
    public float barrier_coolTime_base = 10f;
    [SerializeField] float moveSpeedLvlMultiplier = 0.5f;
    [SerializeField] float eatingDelayLvlMultiplier = 0.05f;
    [SerializeField] float barrierCoolTimeMultiplier = 0.5f;
    [HideInInspector] public float speed_normal;
    [HideInInspector] public float eating_delay;
    [HideInInspector] public float barrier_coolTime;

    [Header("Hard Mode")]
    [SerializeField] SheepStatContainer hardModeStat;

    [Header("Level")]
    [SerializeField] int exp_required_base = 5;
    [SerializeField] int exp_required_multiplier = 5;
    [SerializeField] TextMeshProUGUI valueUI_leg;
    [SerializeField] TextMeshProUGUI valueUI_teeth;
    [SerializeField] TextMeshProUGUI valueUI_bleat;
    [SerializeField] TextMeshProUGUI valueUI_fur;

    [Header("Hit")]
    [SerializeField] private float invincible_duration_hit = 1f;
    [SerializeField] private float hitAlpha = 0.75f;
    private SpriteRenderer sr;
    public bool canHit;
    private int hitCount;


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


    Coroutine invincibleCoroutine;

    void Awake()
    {
        for (int i = 0; i < 12; i++) UI_Hearts.Add(Instantiate(heart_prefab, HP_Parent));
        sr = GetComponent<SpriteRenderer>();
        Reset();
    }

    public void EnterHardMode()
    {
        this.speed_normal_base = hardModeStat.speed_normal_base;
        this.eating_delay_base = hardModeStat.eating_delay_base;
        this.barrier_coolTime_base = hardModeStat.barrier_coolTime_base;
        this.barrierCoolTimeMultiplier = hardModeStat.barrierCoolTimeMultiplier;
    }

    #region EXP & Lvl Up
    public void GainEXP()
    {
        score += 1;
        GameManager.Instance.BalanceGame(score);
        exp += 1;
        UpdateUI_Score();
        TryLevelUp();
    }

    private void UpdateUI_Score()
    {
        text_score.text = "Grass " + score;
    }

    private void TryLevelUp()
    {
        if (exp < exp_required) return;
        if (score >= 100) return;
        GameManager.Instance.PauseGame();

        LevelUpPanel.SetActive(true);

        lvl_sheep += 1;
        exp_required = exp_required_base + lvl_sheep * exp_required_multiplier;
        UpdateUI_Score();
    }

    // Called by buttons on the level up panel
    public void ComponentLevelUp(string component)
    {
        if (lvl_components[component] == 10) return;
        lvl_components[component] += 1;
        UpdateStat();
        GameManager.Instance.ResumeGame();
        LevelUpPanel.SetActive(false);
    }

    private void UpdateStat()
    {
        speed_normal = speed_normal_base + moveSpeedLvlMultiplier * (lvl_components["leg"] - 1);
        eating_delay = eating_delay_base - eatingDelayLvlMultiplier * (lvl_components["teeth"] - 1);
        max_hp = hp_base + (lvl_components["fur"] - 1);
        barrier_coolTime = barrier_coolTime_base - barrierCoolTimeMultiplier * (lvl_components["bleat"] - 1);

        SetHPUI(max_hp - hitCount);
        UpdateUI_Lvl();
    }

    private void UpdateUI_Lvl()
    {
        valueUI_leg.text = lvl_components["leg"].ToString();
        valueUI_teeth.text = lvl_components["teeth"].ToString();
        valueUI_bleat.text = lvl_components["bleat"].ToString();
        valueUI_fur.text = lvl_components["fur"].ToString();

        if (lvl_components["leg"] == 10) valueUI_leg.text = "Max";
        if (lvl_components["teeth"] == 10) valueUI_teeth.text = "Max";
        if (lvl_components["bleat"] == 10) valueUI_bleat.text = "Max";
        if (lvl_components["fur"] == 10) valueUI_fur.text = "Max";
    }
    #endregion

    #region Hit & HP

    public void Hit()
    {
        if (!canHit) return;
        hitCount += 1;
        SetHPUI(max_hp - hitCount);

        if (hitCount >= max_hp)
        {
            GameManager.Instance.GameOver();
            return;
        }
        invincibleCoroutine = StartCoroutine(HitInvincible());
    }

    private System.Collections.IEnumerator HitInvincible()
    {
        canHit = false;
        sr.DOFade(hitAlpha, 0f); // Change alpha immediately
        yield return new WaitForSeconds(invincible_duration_hit);
        sr.DOFade(1f, 0f);
        canHit = true;
    }

    public void StopHitInvincible()
    {
        canHit = true;
        sr.DOFade(1f, 0f);
        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
            invincibleCoroutine = null;
        }
    }

    private void SetHPUI(int currentHP)
    {
        if (currentHP < 0) currentHP = 0;
        for (int i = 0; i < currentHP; i++)
        {
            UI_Hearts[i].SetActive(true);
        }
        for (int i = currentHP; i < UI_Hearts.Count; i++)
        {
            UI_Hearts[i].SetActive(false);
        }
    }

    #endregion

    public void Reset()
    {
        // reset HP
        hitCount = 0;
        max_hp = hp_base;
        SetHPUI(max_hp);

        // reset hit invincible effect
        StopHitInvincible();

        // reset score & EXP
        score = 0;
        exp = 0;
        exp_required = exp_required_base;
        UpdateUI_Score();

        // reset every stat
        lvl_sheep = 0;
        foreach (var key in lvl_components.Keys.ToList()) { lvl_components[key] = 1; }
        UpdateStat();
    }
}