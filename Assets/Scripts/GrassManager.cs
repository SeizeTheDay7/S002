using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    [SerializeField] GameObject grass_prefab;
    List<GameObject> grassPool = new List<GameObject>();
    [SerializeField] private float boundary;
    private float grassDelayTime = 2f;
    bool isGrassReady = true;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            grassPool.Add(MakeNewGrass());
        }
    }

    private GameObject MakeNewGrass()
    {
        GameObject newGrass = Instantiate(grass_prefab);
        newGrass.SetActive(false);
        return newGrass;
    }

    void Update()
    {
        if (isGrassReady) AddGrassToGame();
    }

    private void AddGrassToGame()
    {
        StartCoroutine(GrassCoolDown());
        foreach (GameObject grass in grassPool)
        {
            if (!grass.activeInHierarchy)
            {
                PositionGrass(grass);
                grass.SetActive(true);
                return;
            }
        }

        // If there's no available grass, make new grass
        GameObject newGrass = MakeNewGrass();
        grassPool.Add(newGrass);
        // If there's no available position, don't enable new grass;
        if (!PositionGrass(newGrass)) return;
        newGrass.SetActive(true);
    }

    private IEnumerator GrassCoolDown()
    {
        isGrassReady = false;
        yield return new WaitForSeconds(grassDelayTime);
        isGrassReady = true;
    }

    // return false if it fails to find available position;
    private bool PositionGrass(GameObject grass)
    {
        Vector2 randomPos;
        for (int i = 0; i < 10; i++)
        {
            randomPos = GetRandomPos();
            if (IsValidPos(randomPos))
            {
                grass.transform.position = randomPos;
                return true;
            }
        }

        return false;
    }

    private Vector2 GetRandomPos()
    {
        return new Vector2(Random.Range(-boundary, boundary), Random.Range(-boundary, boundary));
    }

    private bool IsValidPos(Vector3 pos)
    {
        foreach (GameObject grass in grassPool)
        {
            if (grass.activeInHierarchy && grass.transform.position == pos)
            {
                return false;
            }
        }
        return true;
    }

    public void Reset()
    {
        foreach (GameObject grass in grassPool)
        {
            grass.SetActive(false);
        }
        StopAllCoroutines();
        isGrassReady = true;
    }

    public void EndGame()
    {
        enabled = false;
    }
}