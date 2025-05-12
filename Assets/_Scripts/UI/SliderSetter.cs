using SOEventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetter : MonoBehaviour
{
    private enum SliderType
    {
        SFX,
        Music,
        Master
    }

    private Slider slider;
    [SerializeField] private SliderType sliderType;
    [SerializeField] private FloatPublisher onValueChanged;

    private void Start()
    {
        slider = GetComponent<Slider>();
        switch (sliderType)
        {
            case SliderType.SFX:
                slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                break;
            case SliderType.Music:
                slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                break;
            case SliderType.Master:
                slider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
                break;
        }
    }
}
