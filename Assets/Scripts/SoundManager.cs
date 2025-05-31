using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : Singleton<SoundManager>
{
    [Header("BGM")]
    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip inGameBGM;
    [SerializeField] AudioClip thirdWorldBGM;

    [Header("UI Sfx")]
    [SerializeField] AudioClip sfx_Click;
    [SerializeField] AudioClip sfx_GameOver;

    [Header("InGame Sfx")]
    [SerializeField] AudioClip sfx_Hit;
    [SerializeField] AudioClip sfx_Shot;
    [SerializeField] AudioClip sfx_Eat;
    [SerializeField] AudioClip[] sfx_barks;
    [SerializeField] AudioClip sfx_OpenDoor;
    [SerializeField] AudioClip sfx_Bleat;
    List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] private int poolSize = 10;
    [HideInInspector] public AudioSource BGMAudioSource;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }

        BGMAudioSource = gameObject.AddComponent<AudioSource>();
        BGMAudioSource.loop = true;
        BGMAudioSource.clip = titleBGM;
        BGMAudioSource.Play();
    }

    private AudioSource GetAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
                return source;
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }

    public void StopBGM()
    {
        BGMAudioSource.Stop();
    }

    public void PlayBGM()
    {
        BGMAudioSource.Play();
    }

    public void InGameBGM()
    {
        BGMAudioSource.Stop();
        BGMAudioSource.clip = inGameBGM;
        BGMAudioSource.Play();
    }

    public void ThirdWorldBGMFadeIn(float blendTime)
    {
        BGMAudioSource.Stop();
        BGMAudioSource.clip = thirdWorldBGM;
        BGMAudioSource.volume = 0f;
        BGMAudioSource.Play();
        BGMAudioSource.DOFade(1f, blendTime).SetEase(Ease.InOutQuad);
    }

    public void ThirdWorldBGMFadeOut(float blendTime)
    {
        BGMAudioSource.volume = 1f;
        BGMAudioSource.DOFade(0f, blendTime).SetEase(Ease.InOutQuad).OnComplete(() => { BGMAudioSource.Stop(); });
    }

    public void ClickSfx() { PlayInGameAudio(sfx_Click); }
    public void GameOverSfx() { PlayInGameAudio(sfx_GameOver); }
    public void HitSfx() { PlayInGameAudio(sfx_Hit); }
    public void ShotSfx() { PlayInGameAudio(sfx_Shot); }
    public void BarkSfx() { PlayInGameAudio(sfx_barks[Random.Range(0, sfx_barks.Length)]); }
    public void BleatSfx() { PlayInGameAudio(sfx_Bleat); }

    public AudioSource EatSfx(float eating_delay)
    {
        AudioSource source = GetAudioSource();
        source.loop = false;
        source.clip = sfx_Eat;
        source.pitch = sfx_Eat.length / eating_delay;
        source.Play();
        return source;
    }

    public void OpenDoorSfx()
    {
        AudioSource source = GetAudioSource();
        source.loop = false;
        source.clip = sfx_OpenDoor;
        source.Play();
    }

    public void StopAllSound()
    {
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }

    public void PauseAllSound()
    {
        foreach (var source in audioSources)
        {
            source.Pause();
        }
    }

    public void ResumeAllSound()
    {
        foreach (var source in audioSources)
        {
            source.UnPause();
        }
    }

    private void PlayInGameAudio(AudioClip audioClip)
    {
        if (!BGMAudioSource.isPlaying) return;
        AudioSource source = GetAudioSource();
        source.loop = false;
        source.pitch = 1f;
        source.clip = audioClip;
        source.Play();
        if (Time.deltaTime == 0) source.Pause();
    }
}
