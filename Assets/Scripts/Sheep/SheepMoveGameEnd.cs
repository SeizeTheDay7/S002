using UnityEngine;

public class SheepMoveGameEnd : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 10f;
    Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) input += Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow)) input += Vector2.right;
        if (Input.GetKey(KeyCode.UpArrow)) input += Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow)) input += Vector2.down;
        input = input.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveSpeed * input;
    }
}