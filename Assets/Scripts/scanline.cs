using UnityEngine;

public class Scanline : MonoBehaviour
{
    Renderer rdr;
    MaterialPropertyBlock mpb;

    void Start()
    {
        rdr = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        rdr.GetPropertyBlock(mpb);
        mpb.SetFloat("_CustomTime", Time.unscaledTime);
        rdr.SetPropertyBlock(mpb);
    }
}