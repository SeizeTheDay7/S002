using System.Collections;
using UnityEngine;

public class Sheep3Dto2D : MonoBehaviour
{
    [SerializeField] ChangeDimensionMovePanel movepanel;
    [SerializeField] Sheep2Dto3D sheep2Dto3D;
    [SerializeField] GameObject sheep_2d;
    [SerializeField] GameObject sheep_3d;
    bool isCoolTime = false;
    void OnTriggerEnter(Collider other)
    {
        print("Sheep3Dto2D");
        if (isCoolTime) return;
        sheep_3d.SetActive(false);
        StartCoroutine(movepanel.fadeInPanel(sheep_2d, sheep2Dto3D));
    }

    public IEnumerator CoolTime()
    {
        isCoolTime = true;
        yield return new WaitForSeconds(0.1f);
        isCoolTime = false;
    }
}
