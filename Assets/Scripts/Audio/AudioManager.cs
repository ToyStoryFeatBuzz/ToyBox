using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [FormerlySerializedAs("_audioSourceMusic")] public AudioSource AudioSourceMusic;
    [FormerlySerializedAs("_audioSourceSFX")] public AudioSource AudioSourceSFX;

    [FormerlySerializedAs("_musicSounds")] public Sound[] MusicSounds;
    [FormerlySerializedAs("_sfxSounds")] public Sound[] SFXSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlaySound( Sound[] soundList, string name,Vector3 pos=new() ,float sonsLocale=0.0f, float volume=1f)
    {
        Sound sound = Array.Find(soundList, s => s.Name == name);

        if (sound == null) {
            return;
        }
        GameObject tempAudio=new ("tempAudio");
        tempAudio.transform.position=pos;
        AudioSource audioSource= tempAudio.AddComponent<AudioSource>();
        audioSource.spatialBlend=sonsLocale;
        audioSource.clip = sound.Clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(tempAudio,sound.Clip.length);
    }
    
    private void PlaySound(Sound[] soundList,string name, AudioSource audioSource, float volume=1f)
    {
        Sound sound = Array.Find(soundList, s => s.Name == name);

        if (sound == null) {
            return;
        }
        audioSource.clip = sound.Clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
    

    private void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void StopMusic()
    {
        StopSound(AudioSourceMusic);
    }
    
    

    public void StopSFX()
    {
        StopSound(AudioSourceSFX);
    }

    public void PlayMusic(string name, float volume=1f)
    {
        PlaySound(MusicSounds,name, AudioSourceMusic, volume);
    }

    public void PlaySFX(string name, Vector2 pos=new(), float spatialBlend=0.0f, float volume=1.0f)
    {
        PlaySound(SFXSounds,name, pos, spatialBlend);
    }

    public bool IsSoundInList(Sound[] soundList, string name)
    {
        return Array.Exists(soundList, s => s.Name == name);
    }
}
