using UnityEngine;
using System.Collections;

public class DummySheep : MonoBehaviour
{
    [SerializeField] GameObject barrier;
    Rigidbody2D rb;
    private float moving_speed = 5f;
    private bool clickedRecently = false;
    private Vector2 lastInput = Vector2.zero;
    float clickDelay = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) input += Vector2.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) input += Vector2.right;
        if (Input.GetKeyDown(KeyCode.UpArrow)) input += Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) input += Vector2.down;

        if (input != Vector2.zero)
        {
            if (clickedRecently)
            {
                lastInput += input;
                // prevent zero velocity
                if (lastInput == Vector2.zero) lastInput = input;

                rb.linearVelocity = lastInput.normalized * moving_speed;
            }
            else
            {
                rb.linearVelocity = input.normalized * moving_speed;
                StartCoroutine(waitingOtherClick(input));
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(FakeBarrier());
        }
    }

    private IEnumerator FakeBarrier()
    {
        barrier.SetActive(true);
        yield return new WaitForSeconds(1f);
        barrier.SetActive(false);
    }

    void FixedUpdate()
    {
        // 지속적으로 FixedUpdate에서 안전하게 속도 적용
        if (lastInput != Vector2.zero)
        {
            rb.linearVelocity = lastInput.normalized * moving_speed;
        }
    }

    private IEnumerator waitingOtherClick(Vector2 input)
    {
        clickedRecently = true;
        lastInput = input;
        yield return new WaitForSeconds(clickDelay);
        clickedRecently = false;
    }
}