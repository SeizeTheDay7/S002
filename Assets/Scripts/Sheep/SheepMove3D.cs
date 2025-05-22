using UnityEngine;

public class SheepMove3D : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 move = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.RightArrow)) move.x += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) move.x -= 1;

        cc.SimpleMove(move * moveSpeed);
    }
}
