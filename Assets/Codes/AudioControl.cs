using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    private AudioMixer mixer;
    public Slider slider;
    private string volumeName;

    public void Setup(AudioMixer mixer, string volumeName)
    {
        this.mixer = mixer;
        this.volumeName = volumeName;

        if (mixer.GetFloat(volumeName, out float volume))
        {
            slider.value = ToLinear(volume);
        }
        slider.onValueChanged.AddListener(delegate { OnValueChanged(slider); });
    }

    private void OnValueChanged(Slider slider)
    {
        mixer.SetFloat(volumeName, ToDB(slider.value));
    }

    private float ToDB(float linear)
    {
        return linear <= 0 ? -144.0f : 20f * Mathf.Log10(linear);
    }

    private float ToLinear(float dB)
    {
        return Mathf.Clamp01(Mathf.Pow(10.0f, dB / 20.0f));
    }
}