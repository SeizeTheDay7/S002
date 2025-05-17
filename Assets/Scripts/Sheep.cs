using UnityEngine;

public class Sheep : Singleton<Sheep>
{
    [SerializeField] SheepMove sheepMove;
    [SerializeField] SheepStat sheepStat;

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.GetComponent<Grass>())
        {
            sheepMove.EatingGrass(obj);
        }
        else if (obj.GetComponent<Wolf>())
        {
            obj.GetComponent<Wolf>().bitSheep = true;
            sheepStat.Hit();
        }
    }
}