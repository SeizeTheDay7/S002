using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PillarBoxUI : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Renderer backgroundR;
    [SerializeField] RectTransform leftMask, rightMask;

    Canvas canvas; float prevW, prevH;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void OnEnable() => Refresh();
    void LateUpdate()
    {
        if (Screen.width != prevW || Screen.height != prevH) Refresh();
    }

    void Refresh()
    {
        if (!backgroundR) return;

        float pillarUI = (Screen.width - Screen.height) * 0.5f;

        bool show = pillarUI > 0.5f;
        leftMask.gameObject.SetActive(show);
        rightMask.gameObject.SetActive(show);

        if (show)
        {
            leftMask.sizeDelta = new Vector2(pillarUI, 0);
            rightMask.sizeDelta = new Vector2(pillarUI, 0);
        }

        prevW = Screen.width;
        prevH = Screen.height;
    }
}
