using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    [SerializeField] GameObject wolf_prefab;
    [SerializeField] private float boundary;
    [SerializeField] private float wolfSpeed = 10f;
    [SerializeField] private float wolfDealyTime = 2f;
    [HideInInspector] public float chaseDuration = 0f;
    [HideInInspector] public int wolfAmount = 1;

    private bool isWolfReady = true;
    private List<GameObject> wolfPool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            wolfPool.Add(MakeNewWolf());
        }
    }

    public void EnterHardMode()
    {
        wolfSpeed = 10f;
    }

    private GameObject MakeNewWolf()
    {
        GameObject newWolf = Instantiate(wolf_prefab);
        newWolf.SetActive(false);
        return newWolf;
    }

    void Update()
    {
        if (isWolfReady) AddWolfToGame();
    }

    private void AddWolfToGame()
    {
        StartCoroutine(WolfCoolDown());
        for (int i = 0; i < wolfAmount; i++)
        {
            bool foundWolf = false;
            foreach (GameObject wolf in wolfPool)
            {
                if (!wolf.activeInHierarchy)
                {
                    InitWolf(wolf);
                    wolf.SetActive(true);
                    foundWolf = true;
                    break;
                }
            }
            // If there's no free wolf, make new wolf
            if (!foundWolf)
            {
                GameObject newWolf = MakeNewWolf();
                InitWolf(newWolf);
                wolfPool.Add(newWolf);
            }
        }
    }

    private IEnumerator WolfCoolDown()
    {
        isWolfReady = false;
        yield return new WaitForSeconds(wolfDealyTime);
        isWolfReady = true;
    }

    private void InitWolf(GameObject wolf)
    {
        wolf.GetComponent<Wolf>().InitWolf(GetRandomPos(), wolfSpeed, chaseDuration, boundary);
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
        foreach (GameObject wolf in wolfPool)
        {
            wolf.SetActive(false);
            wolf.GetComponent<Wolf>().Reset();
        }

        StopAllCoroutines();

        isWolfReady = true;
        wolfAmount = 1;
        chaseDuration = 0;
    }

    public void EndGame()
    {
        Reset();
        enabled = false;
    }
}
