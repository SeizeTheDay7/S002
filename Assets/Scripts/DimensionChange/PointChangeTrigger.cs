using UnityEngine;

public class PointChangeTrigger : MonoBehaviour
{
    [SerializeField] Camera2DPointChange changeScript;
    [SerializeField] Transform point;
    [SerializeField] GameObject otherTrigger;

    void OnTriggerEnter2D(Collider2D collision)
    {
        changeScript.MoveInto(point);
        otherTrigger.SetActive(true);
        gameObject.SetActive(false);
    }
}
