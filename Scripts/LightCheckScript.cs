using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightCheckScript : MonoBehaviour
{
    public RenderTexture lightCheckTexture;
    public float lightLevel;
    //private Slider sanity;

    public float GetLightLevel()
    {
        return lightLevel;
    }

    //void Start()
    //{
    //    sanity = GameObject.Find("SanitySlider").GetComponent<Slider>();
    //}

    // Update is called once per frame
    void Update()
    {
        RenderTexture tempTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tempTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tempTexture;

        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tempTexture.width, tempTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tempTexture);

        Color32[] colors = temp2DTexture.GetPixels32();
        Destroy(temp2DTexture);

        lightLevel = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            lightLevel += (0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b);
        }
        //sanity.value = lightLevel;
        //Debug.Log(lightLevel);
    }
}
