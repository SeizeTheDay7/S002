using UnityEngine;

public class RenderTextureSetup : MonoBehaviour
{
    [SerializeField] Renderer rrr;
    [SerializeField] Camera camera2D;
    [SerializeField] Material renderMat;

    void Start()
    {
        if (camera2D.targetTexture != null) camera2D.targetTexture.Release();
        camera2D.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        renderMat.mainTexture = camera2D.targetTexture;
        rrr.material = renderMat;
    }
}