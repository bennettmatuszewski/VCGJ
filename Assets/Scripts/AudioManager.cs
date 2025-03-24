using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public Sound2[] sounds;
    private static Dictionary<string, float> soundTimerDictionary;
    [HideInInspector] public string currentSongPlaying;
    [HideInInspector] public bool isCrossfading;

    public static AudioManager instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        soundTimerDictionary = new Dictionary<string, float>();

        foreach (Sound2 sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;

        }
        DontDestroyOnLoad(this.gameObject);
        //Debug.Log(Array.FindIndex(sounds, s => s.name == "slimeHit"));
        
    }

    public void Play(string name, float time=0.0f)
    {
        Sound2 sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }
        if (!CanPlaySound(sound))return;

        if (sound.randomizePitch)
        {
            sound.source.pitch = Random.Range(0.8f, 1.3f);
        }
        sound.source.time = time;
        sound.source.Play();
    }
    public void PlaySong(string name, float time=0.0f)
    {
        Sound2 sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        if (!CanPlaySound(sound))return;

        sound.source.time = time;
        sound.source.Play();
        currentSongPlaying = name;
    }

    public void Stop(string name)
    {
        Sound2 sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        sound.source.Stop();
    }
    public bool IsPlaying (string name)
    {
        Sound2 sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return false;
        }

        return sound.source.isPlaying;
    }

    private static bool CanPlaySound(Sound2 sound)
    {
        if (sound.source==null)
        {
            return false;
        }
        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            float lastTimePlayed = soundTimerDictionary[sound.name];

            if (lastTimePlayed + sound.clip.length < Time.time)
            {
                soundTimerDictionary[sound.name] = Time.time;
                return true;
            }

            return false;
        }

        return true;
    }
    public void Fade(String clipName, float duration, float targetVolume)
    {
        Sound2 audioSource = Array.Find(sounds, s => s.name == clipName);
        DOTween.To(() => audioSource.source.volume, x => audioSource.source.volume = x, targetVolume, duration).OnComplete(()=>audioSource.volume = audioSource.source.volume);
    }

    public void CrossFade(String fadeOutFlip, String fadeInClip, float duration)
    {
        Sound2 fadeOut = Array.Find(sounds, s => s.name == fadeOutFlip);
        Sound2 fadeIn = Array.Find(sounds, s => s.name == fadeInClip);
        float tmp = fadeIn.volume;
        float tmp2 = fadeOut.volume;
        fadeIn.volume = 0;
        fadeIn.source.volume = 0;
        Play(fadeInClip);
        DOTween.To(() => fadeOut.source.volume, x => fadeOut.source.volume = x, 0, duration).OnComplete(()=>FinishCrossFade(fadeOut,tmp2));
        DOTween.To(() => fadeIn.source.volume, x => fadeIn.source.volume = x, tmp, duration).OnComplete(()=>fadeIn.volume = fadeIn.source.volume);
        currentSongPlaying = fadeInClip;
    }
    void FinishCrossFade(Sound2 clip, float ogVolume)
    {
        Stop(clip.name);
        clip.volume = ogVolume;
        isCrossfading = false;
    }
       public void FadeOutStop(String clipName, float duration)
       {
           Sound2 audioSource = Array.Find(sounds, s => s.name == clipName);
           float tmp = audioSource.volume;
           DOTween.To(() => audioSource.source.volume, x => audioSource.source.volume = x, 0, duration).OnComplete(()=>FadeOutStopEnd(audioSource,tmp, clipName));
       }
       private void FadeOutStopEnd(Sound2 aSound2, float v, String clipname)
       {
           aSound2.source.volume = v;
           Stop(clipname);
       }
}