using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    [SerializeField] GameObject sheep;
    [SerializeField] GameObject wolf_prefab;
    [SerializeField] private float boundary;
    private bool isWolfReady = true;
    private float wolfDealyTime = 2f;
    private List<GameObject> wolfPool = new List<GameObject>();
    private Coroutine coolDownCoroutine;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            wolfPool.Add(MakeNewWolf());
        }
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
        coolDownCoroutine = StartCoroutine(WolfCoolDown());
        foreach (GameObject wolf in wolfPool)
        {
            if (!wolf.activeInHierarchy)
            {
                ChaseSheep(wolf);
                wolf.SetActive(true);
                return;
            }
        }

        // If there's no available wolf, make new wolf
        GameObject newWolf = MakeNewWolf();
        ChaseSheep(newWolf);
        wolfPool.Add(newWolf);
    }

    private IEnumerator WolfCoolDown()
    {
        isWolfReady = false;
        yield return new WaitForSeconds(wolfDealyTime);
        isWolfReady = true;
    }

    private void ChaseSheep(GameObject wolf)
    {
        wolf.transform.position = GetRandomPos();
        wolf.GetComponent<Wolf>().ChaseSheep(sheep.transform.position, boundary);
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
        if (coolDownCoroutine != null)
        {
            StopCoroutine(coolDownCoroutine);
            coolDownCoroutine = null;
        }
        isWolfReady = true;
        wolfDealyTime = 2f;
    }
}
