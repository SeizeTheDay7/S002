using UnityEngine;
using DG.Tweening;

public class Wolf : MonoBehaviour
{
    private Vector2 dir;
    private float boundary;
    private float startTime = 0;
    private float speed;
    private float chaseDuration;
    private float marchAmount = 0.5f;
    private bool canMove = false;
    public bool bitSheep = false;
    private Tween currentTween;
    private Transform sheep;

    public void InitWolf(Vector2 pos, float speed, float chaseDuration, float boundary)
    {
        transform.position = pos;
        this.speed = speed;
        this.chaseDuration = chaseDuration;
        this.boundary = boundary - 2;

        canMove = false;
        bitSheep = false;
        startTime = 0;

        sheep = Sheep.Instance.transform;
        dir = (sheep.position - transform.position).normalized;

        transform.DOJump(GetInsideJumpPos(), 2f, 1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => canMove = true);
    }

    void Update()
    {
        if (canMove)
        {
            // change direction for a few sceconds. ends if hit by sheep.
            if (startTime < chaseDuration && !bitSheep)
            {
                startTime += Time.deltaTime;
                dir = (sheep.position - transform.position).normalized;
            }
            transform.position += (Vector3)dir * speed * Time.deltaTime;
            if (Time.deltaTime != 0 && IsOutside(transform.position))
            {
                currentTween = transform.DOJump(GetOutsideJumpPos(), 2f, 1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
                canMove = false;
            }
        }
    }

    private Vector2 GetInsideJumpPos()
    {
        Vector2 jumpPos = transform.position;
        // Find inside position by raymarching
        while (IsOutside(jumpPos))
        {
            jumpPos += marchAmount * dir;
        }
        return jumpPos;
    }

    private Vector2 GetOutsideJumpPos()
    {
        this.boundary += 2;
        Vector2 jumpPos = transform.position;
        while (!IsOutside(jumpPos))
        {
            jumpPos += marchAmount * dir;
        }
        return jumpPos;
    }

    private bool IsOutside(Vector2 pos)
    {
        return Mathf.Abs(pos.x) > boundary || Mathf.Abs(pos.y) > boundary;
    }

    public void Reset()
    {
        currentTween?.Kill();
        currentTween = null;
    }
}
