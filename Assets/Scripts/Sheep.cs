using UnityEngine;

public class Sheep : Singleton<Sheep>
{
    [SerializeField] SheepMove sheepMove;
    [SerializeField] SheepStat sheepStat;
    private System.Action<Collider2D> triggerHandler;

    void Start()
    {
        triggerHandler = DefaultTriggerHandler;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        triggerHandler?.Invoke(collision);
    }

    void DefaultTriggerHandler(Collider2D collision)
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
        else if (obj.GetComponent<Bullet>())
        {
            obj.transform.parent.gameObject.SetActive(false);
            sheepStat.Hit();
        }
    }

    private void GameEndTriggerHandler(Collider2D collision)
    {
        // nothing happens here
    }

    public void EndGame()
    {
        triggerHandler = GameEndTriggerHandler;
    }
}