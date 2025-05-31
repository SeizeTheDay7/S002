using UnityEngine;
using System.Collections;

public class SheepBarrier : MonoBehaviour
{
    [SerializeField] private SheepStat sheepStat;
    [SerializeField] private GameObject barrier;
    [SerializeField] private float barrier_duration = 1f;
    [SerializeField] private GameObject barrier_UI;
    private bool canBarrier = true;
    private Coroutine barrierCoroutine;
    SoundManager soundManager;

    void Start()
    {
        barrier_UI.SetActive(true);
        soundManager = SoundManager.Instance;
    }

    void Update()
    {
        if (canBarrier)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.deltaTime != 0)
            {
                barrierCoroutine = StartCoroutine(Barrier());
            }
        }
    }

    private IEnumerator Barrier()
    {
        soundManager.BleatSfx();
        sheepStat.StopHitInvincible();
        sheepStat.canHit = false;
        barrier.SetActive(true);
        barrier_UI.SetActive(false);
        canBarrier = false;

        yield return new WaitForSeconds(barrier_duration);
        sheepStat.canHit = true;
        barrier.SetActive(false);

        yield return new WaitForSeconds(sheepStat.barrier_coolTime);
        canBarrier = true;
        barrier_UI.SetActive(true);
    }

    public void Reset()
    {
        canBarrier = true;
        barrier_UI.SetActive(true);
        barrier.SetActive(false);
        if (barrierCoroutine != null)
        {
            StopCoroutine(barrierCoroutine);
            barrierCoroutine = null;
        }
    }



}