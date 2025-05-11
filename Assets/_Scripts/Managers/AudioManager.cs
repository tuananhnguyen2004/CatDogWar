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
    }
}
