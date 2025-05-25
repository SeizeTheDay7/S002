using UnityEngine;
using DG.Tweening;

public class Camera2DPointChange : MonoBehaviour
{
    [SerializeField] GameObject Camera2D;
    Tween currentTween;

    public void MoveInto(Transform point)
    {
        if (currentTween != null) currentTween.Kill();
        currentTween = Camera2D.transform.DOMove(point.position, 0.5f);
    }

}