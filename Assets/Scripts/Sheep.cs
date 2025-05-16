using UnityEngine;

public class Sheep : MonoBehaviour
{
    [SerializeField] SheepMove sheepMove;

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.GetComponent<Grass>())
        {
            print("Eat Grass");
            sheepMove.EatingGrass(obj);
        }
        else if (obj.GetComponent<Wolf>())
        {
            print("Eaten by wolf");
            GameManager.Instance.GameOver();
        }
    }
}