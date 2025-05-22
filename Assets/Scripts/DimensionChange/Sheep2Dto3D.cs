using System.Collections;
using UnityEngine;

public class Sheep2Dto3D : MonoBehaviour
{
    [SerializeField] ChangeDimensionMovePanel movepanel;
    [SerializeField] Sheep3Dto2D sheep3Dto2D;
    [SerializeField] GameObject sheep_2d;
    [SerializeField] GameObject sheep_3d;
    bool isCoolTime = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        print("Sheep2Dto3D");
        if (isCoolTime) return;
        sheep_3d.SetActive(true);
        sheep_2d.SetActive(false);
        movepanel.fadeOutPanel();
        StartCoroutine(sheep3Dto2D.CoolTime());
    }

    public IEnumerator CoolTime()
    {
        isCoolTime = true;
        yield return new WaitForSeconds(0.1f);
        isCoolTime = false;
    }
}