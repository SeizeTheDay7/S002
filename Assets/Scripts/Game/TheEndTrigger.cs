using System.Collections;
using UnityEngine;

public class TheEndTrigger : MonoBehaviour
{
    [SerializeField] GameObject sheep_3d;
    [SerializeField] GameObject renderTexture_2d;
    [SerializeField] GameObject TheEnd;
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EndingCoroutine());
    }

    IEnumerator EndingCoroutine()
    {
        sheep_3d.SetActive(false);
        yield return new WaitForSeconds(3f);
        renderTexture_2d.SetActive(false);
        yield return new WaitForSeconds(3f);
        TheEnd.SetActive(true);
        yield return new WaitForSeconds(5f);
        print("Quit");
        Application.Quit();
    }
}
