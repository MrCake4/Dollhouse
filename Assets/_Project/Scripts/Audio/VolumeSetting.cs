using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;

    void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }
}
