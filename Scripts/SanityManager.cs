using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class SanityManager : MonoBehaviour
{
    Slider sanitySlider;
    public PostProcessProfile profile;
    Vignette vignette;
    public int fullSanity;
    public LightCheckScript lightCheckScript;
    private float lightLevel;
    float percent;

    // Start is called before the first frame update
    void Start()
    {
        profile.TryGetSettings(out vignette);
        sanitySlider = GetComponent<Slider>();
        sanitySlider.maxValue = fullSanity;
        sanitySlider.value = fullSanity;
        vignette.intensity.value = 0;
        //lightCheckScript = GetComponent<LightCheckScript>();

        StartCoroutine(LoseSanity());
    }

    IEnumerator LoseSanity()
    {
        //Debug.Log(lightCheckScript.lightLevel);
        while(true)
        {
            lightLevel = lightCheckScript.lightLevel;
            if(lightLevel < 48000)
            {
                Debug.Log("It's too dark! I need light!");
                sanitySlider.value -= 2f * 8;
            }
            else
            {
                Debug.Log("I like light!");
                sanitySlider.value += 2f * 30;
            }
            float newValue = (sanitySlider.value - sanitySlider.maxValue) * -1;
            percent = newValue / sanitySlider.maxValue;
            vignette.intensity.value = percent;
            yield return null;
        }
    }
}
