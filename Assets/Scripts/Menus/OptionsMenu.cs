using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("VolumeMaster", volume);
    }

    public void SetName (string name)
    {
        // TODO: Call function to set player name here..
    }
}
