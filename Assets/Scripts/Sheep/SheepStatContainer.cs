using UnityEngine;

[CreateAssetMenu(fileName = "SheepStat", menuName = "GameData/SheepStat")]
public class SheepStatContainer : ScriptableObject
{
    public float speed_normal_base = 15f;
    public float eating_delay_base = 0.55f;
    public float barrier_coolTime_base = 10f;
    public float barrierCoolTimeMultiplier = 0.9f;
}