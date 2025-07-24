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

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }


    void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        Save();
    }

    void Load()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", masterVolumeSlider.value);
    }
}
