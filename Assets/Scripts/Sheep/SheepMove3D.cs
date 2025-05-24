using UnityEngine;
using System.Collections;

public class SheepMove3D : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    CharacterController cc;
    bool turning;
    bool isRight = true;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (turning) return;
        Vector2 move = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move.x += 1;
            if (!isRight)
            {
                StartCoroutine(Turn(1));
                return;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.x -= 1;
            if (isRight)
            {
                StartCoroutine(Turn(-1));
                return;
            }
        }

        cc.SimpleMove(move * moveSpeed);
    }

    IEnumerator Turn(int direction)
    {
        turning = true;
        float targetY = (direction == 1) ? 90f : 270f;
        Quaternion targetRotation = Quaternion.Euler(0, targetY, 0);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.rotation = targetRotation;

        isRight = !isRight;
        turning = false;
    }

}
