using System.Collections;
using UnityEngine;

public class SheepMove : MonoBehaviour
{
    [SerializeField] private SheepStat stat;
    private float moving_speed;
    private Rigidbody2D rb;
    private bool clickedRecently = false;
    private Vector2 lastInput = Vector2.zero;
    [SerializeField] float clickDelay = 0.05f;
    ParticleSystem ps;
    TrailRenderer tr;
    Coroutine eatingCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        tr = GetComponent<TrailRenderer>();
        moving_speed = stat.speed_normal;
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
                if (eatingCoroutine != null)
                {
                    moving_speed = stat.speed_normal;
                    ps.Stop();

                    StopCoroutine(eatingCoroutine);
                    eatingCoroutine = null;
                }
                rb.linearVelocity = input.normalized * moving_speed;
                StartCoroutine(waitingOtherClick(input));
            }
        }
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

    public void EatingGrass(GameObject grass)
    {
        if (eatingCoroutine != null) return;

        moving_speed = stat.speed_eating;
        if (!ps.isPlaying)
        {
            var main = ps.main;
            main.duration = stat.eating_delay;
        }
        ps.Play();
        eatingCoroutine = StartCoroutine(EatingCoroutine(grass));
    }

    private IEnumerator EatingCoroutine(GameObject grass)
    {
        yield return new WaitForSeconds(stat.eating_delay);

        moving_speed = stat.speed_normal;
        ps.Stop();

        grass.SetActive(false);
        eatingCoroutine = null;

        stat.GainEXP();
    }

    public void Reset()
    {
        transform.position = Vector2.zero;

        rb.linearVelocity = Vector2.zero;
        moving_speed = stat.speed_normal;

        lastInput = Vector2.zero;
        clickedRecently = false;

        if (eatingCoroutine != null)
        {
            StopCoroutine(eatingCoroutine);
            eatingCoroutine = null;
        }

        ps.Stop();
        ps.Clear();
        tr.Clear();
    }

    public void EndGame()
    {
        tr.Clear();
        tr.enabled = false;

        rb.linearVelocity = Vector2.zero;
        enabled = false;
    }
}
