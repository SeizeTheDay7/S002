using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    [SerializeField] private Transform sheep;
    [SerializeField] private float boundary;
    [SerializeField] private GameObject shot_prefab;
    [SerializeField] private float shotDelay = 1.5f;
    [SerializeField] private float reloadTime_base = 4f;
    [SerializeField] private float bulletSpeed = 20f;
    private List<GameObject> shotPool = new List<GameObject>();
    bool shotReady = true;
    [HideInInspector] public float reloadTime;
    SoundManager soundManager;

    void Awake()
    {
        reloadTime = reloadTime_base;
        for (int i = 0; i < 5; i++)
        {
            shotPool.Add(MakeNewBullet());
        }
    }

    void Start()
    {
        soundManager = SoundManager.Instance;
    }

    private GameObject MakeNewBullet()
    {
        GameObject newShot = Instantiate(shot_prefab);
        newShot.SetActive(false);
        return newShot;
    }

    void Update()
    {
        if (shotReady)
        {
            AddShotToGame();
        }
    }

    private void AddShotToGame()
    {
        StartCoroutine(ShotCoolDown());

        foreach (GameObject shot in shotPool)
        {
            if (!shot.activeInHierarchy)
            {
                InitShot(shot);
                return;
            }
        }

        // If there's no available shot, make new shot
        GameObject newShot = MakeNewBullet();
        shotPool.Add(newShot);
        InitShot(newShot);
    }

    private IEnumerator ShotCoolDown()
    {
        shotReady = false;
        yield return new WaitForSeconds(reloadTime);
        shotReady = true;
    }

    private void InitShot(GameObject shot)
    {
        shot.SetActive(true);
        shot.GetComponent<Shot>().ReadyShot(GetRandomPos(), shotDelay, sheep, bulletSpeed);
        soundManager.ShotSfx();
    }

    private Vector3 GetRandomPos()
    {
        int xSign = (Random.Range(0, 2) == 0) ? -1 : 1;
        int ySign = (Random.Range(0, 2) == 0) ? -1 : 1;
        bool isVertical = Random.Range(0, 2) == 0;

        Vector3 randomPos;
        if (isVertical) randomPos = new Vector3(xSign * boundary, Random.Range(-boundary, boundary), 0);
        else randomPos = new Vector3(Random.Range(-boundary, boundary), ySign * boundary, 0);

        return randomPos;
    }

    public void Reset()
    {
        foreach (GameObject shot in shotPool)
        {
            shot.SetActive(false);
        }
        StopAllCoroutines();
        shotReady = true;
        reloadTime = reloadTime_base;

        enabled = false;
    }

    public void EndGame()
    {
        Reset();
    }
}