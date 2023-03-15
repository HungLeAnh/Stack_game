using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBackground : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.RawImage img;
    [SerializeField]
    private Material fog;

    private Texture2D backgroundTexture;
    void Awake()
    {
        backgroundTexture = new Texture2D(1, 2);
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
        GameManager.OnRunning += GameManager_OnRunning;

    }
    private void OnDestroy()
    {
        GameManager.OnRunning -= GameManager_OnRunning;

    }
    private void GameManager_OnRunning(Color currentColor,Color nextColor)
    {
        Debug.Log($"{currentColor} , { nextColor}");
        SetColor(currentColor, nextColor);
        img.color = currentColor;
        fog.SetColor("Fog Color", currentColor);

    }

    public void SetColor(Color color1, Color color2)
    {
        backgroundTexture.SetPixels(new Color[] { color1, color2 });
        backgroundTexture.Apply();
        img.texture = backgroundTexture;
    }
}
