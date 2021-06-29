using UnityEngine;

public class PixelShader : MonoBehaviour
{
    public Material colourMaterial;
    public Material identityMaterial;

    private RenderTexture _downscaledRenderTexture;

    private void OnEnable()
    {
        var camera = GetComponent<Camera>();
        int height = 144;
        int width = Mathf.RoundToInt(camera.aspect * height);
        _downscaledRenderTexture = new RenderTexture(width, height, 16)
        {
            filterMode = FilterMode.Point
        };
    }

    private void OnDisable()
    {
        Destroy(_downscaledRenderTexture);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, _downscaledRenderTexture, colourMaterial);
        Graphics.Blit(_downscaledRenderTexture, dst, identityMaterial);
    }
}
