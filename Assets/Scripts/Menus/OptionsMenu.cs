using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField nameBox;
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    public void Start()
    {
        string name = PlayerPrefs.GetString("name");
        if (name != null || name != "")
        {
            nameBox.text = name;
        } else
        {
            nameBox.text = "player";
        }
        float volume = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = volume;
    }

    public void SetVolume (float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        audioMixer.SetFloat("VolumeMaster", volume);
    }

    public void SetName ()
    {
        PlayerPrefs.SetString("name", nameBox.text);
    }
}
