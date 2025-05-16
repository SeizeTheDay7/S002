using UnityEngine;
using DG.Tweening;

public class Wolf : MonoBehaviour
{
    private Vector2 dir;
    private float boundary;
    private float speed = 10f;
    private float marchAmount = 0.5f;
    private bool canMove = false;
    private Tween currentTween;

    void OnEnable()
    {
        canMove = false;
    }

    void Update()
    {
        if (canMove)
        {
            transform.position += (Vector3)dir * speed * Time.deltaTime;
            if (Time.deltaTime != 0 && IsOutside(transform.position))
            {
                currentTween = transform.DOJump(GetOutsideJumpPos(), 2f, 1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
                canMove = false;
            }
        }
    }

    public void ChaseSheep(Vector2 sheepPos, float boundary)
    {
        dir = (sheepPos - (Vector2)transform.position).normalized;
        this.boundary = boundary - 2;
        transform.DOJump(GetInsideJumpPos(), 2f, 1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => canMove = true);
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
    }
}
