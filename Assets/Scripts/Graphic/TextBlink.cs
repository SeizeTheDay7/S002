using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextBlink : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float blinkDuration = 0.5f;

    void Start()
    {
        if (textMeshPro == null)
            textMeshPro = GetComponent<TMP_Text>();

        textMeshPro.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}

