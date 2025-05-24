using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ChangeDimensionMovePanel : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;
    [SerializeField] CinemachineCamera vcam2D;
    [SerializeField] CinemachineCamera vcam3D;
    [SerializeField] RectTransform panel_1;
    [SerializeField] RectTransform panel_2;
    [SerializeField] GameObject world_3d;


    public IEnumerator fadeOutPanel(GameObject sheep_3d, Sheep3Dto2D sheep3Dto2D)
    {
        StopAllCoroutines();

        vcam2D.Priority = 0;
        vcam3D.Priority = 10;
        world_3d.SetActive(true);

        float blendTime = brain.DefaultBlend.Time;
        StartCoroutine(AnimatePanel(panel_1, panel_1.anchoredPosition.x, -panel_1.rect.width, blendTime));   // 왼쪽으로
        StartCoroutine(AnimatePanel(panel_2, panel_2.anchoredPosition.x, panel_2.rect.width, blendTime));    // 오른쪽으로
        yield return new WaitForSeconds(blendTime);
        sheep_3d.SetActive(true);
        StartCoroutine(sheep3Dto2D.CoolTime());
    }

    public IEnumerator fadeInPanel(GameObject sheep_2d, Sheep2Dto3D sheep2Dto3D)
    {
        StopAllCoroutines();

        vcam2D.Priority = 10;
        vcam3D.Priority = 0;

        float blendTime = brain.DefaultBlend.Time;
        StartCoroutine(AnimatePanel(panel_1, panel_1.anchoredPosition.x, 0, blendTime));   // 원래 위치로
        StartCoroutine(AnimatePanel(panel_2, panel_2.anchoredPosition.x, 0, blendTime));    // 원래 위치로
        yield return new WaitForSeconds(blendTime);
        sheep_2d.SetActive(true);
        StartCoroutine(sheep2Dto3D.CoolTime());
        world_3d.SetActive(false);
    }

    IEnumerator AnimatePanel(RectTransform panel, float fromX, float toX, float duration)
    {
        Vector2 startPos = new Vector2(fromX, panel.anchoredPosition.y);
        Vector2 endPos = new Vector2(toX, panel.anchoredPosition.y);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        panel.anchoredPosition = endPos;
    }
}