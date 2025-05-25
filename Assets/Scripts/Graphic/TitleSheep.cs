using UnityEngine;

public class TitleSheep : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2[] directions = {
            new Vector2(-1, 1).normalized,  // 좌상
            new Vector2(-1, -1).normalized, // 좌하
            new Vector2(1, 1).normalized,   // 우상
            new Vector2(1, -1).normalized   // 우하
        };
        Vector2 randomDir = directions[Random.Range(0, directions.Length)];
        rb.linearVelocity = randomDir * speed;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
