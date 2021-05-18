using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;
    public Slider slider;
    public string groupName;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(groupName, sliderValue * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}
