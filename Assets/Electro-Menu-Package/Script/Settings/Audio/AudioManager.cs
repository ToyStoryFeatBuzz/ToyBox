using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource _audioSourceMusic;
    public AudioSource _audioSourceSFX;

    public Sound[] _musicSounds;
    public Sound[] _sfxSounds;

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
        Sound sound = Array.Find(soundList, s => s._name == name);
    
        if (sound == null)
        {
            Debug.Log($"{name} Not Found");
        }
        else
        {
            GameObject tempAudio=new GameObject("tempAudio");
            tempAudio.transform.position=pos;
            AudioSource audioSource= tempAudio.AddComponent<AudioSource>();
            audioSource.spatialBlend=sonsLocale;
            audioSource.clip = sound._clip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(tempAudio,sound._clip.length);
          
        }
    }
    
    private void PlaySound(Sound[] soundList,string name, AudioSource audioSource, float volume=1f)
    {
        Sound sound = Array.Find(soundList, s => s._name == name);
    
        if (sound == null)
        {
            Debug.Log($"{name} Not Found");
        }
        else
        {
            audioSource.clip = sound._clip;
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    

    private void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void StopMusic()
    {
        StopSound(_audioSourceMusic);
    }
    
    

    public void StopSFX()
    {
        StopSound(_audioSourceSFX);
    }

    public void PlayMusic(string name, float volume=1f)
    {
        PlaySound(_musicSounds,name, _audioSourceMusic, volume);
    }

    public void PlaySFX(string name, Vector2 pos=new(), float spatialBlend=0.0f, float volume=1.0f)
    {
        PlaySound(_sfxSounds,name, pos, spatialBlend);
    }

    public bool IsSoundInList(Sound[] soundList, string name)
    {
        return Array.Exists(soundList, s => s._name == name);
    }
}
