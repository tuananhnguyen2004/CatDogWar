using SOEventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<Sound> sfxList;
    [SerializeField] private List<Sound> musicList;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private void Start()
    {
        PlayMusic("MainTheme");

        // Load audio data
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    public void PlayMusic(string music)
    {
        AudioClip clip = musicList.Find(m => m.name == music).clip;
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music {music} not found in the list.");
        }
    }

    public void PlaySoundFX(string sfx)
    {
        AudioClip clip = sfxList.Find(x => x.name == sfx).clip;
        if (clip != null)
        {
            Debug.Log("Play sfx: " + clip.name);
            sfxSource.Stop(); // Stop any currently playing sound effect
            sfxSource.clip = clip;
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound FX {sfx} not found in the list.");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
