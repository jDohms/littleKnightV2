using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource AudioS;
    public AudioSource MusicS;
    public AudioSource Steps;
    public AudioSource Chains;
    public AudioSource GameOverS;

    public AudioSource BossMusic;



    #region SFX
    [Header("SFX")]

    public AudioClip clickMenu;
    public AudioClip Heal;
    public AudioClip Attack;
    public AudioClip Hit;
    public AudioClip BossDeath;
    public AudioClip magic;



    #endregion

    void Start () {

        AudioS = GetComponent<AudioSource>();
    }

    void PlayHitFX()
    {
        AudioS.PlayOneShot(Hit, 0.3f);
    }

    void PlayMenuClick()
    {
        AudioS.PlayOneShot(clickMenu, 0.3f);
    }

    void PlayHeal()
    {
        AudioS.PlayOneShot(Heal, 0.3f);
    }

    void PlaySteps()
    {
        if (!Steps.isPlaying)
        {
            Steps.Play();
        }
    }

    void PlayChain()
    {
        if (!Chains.isPlaying)
        {
            Chains.Play();
        }
    }


    void PlayGameOver()
    {
        if (!GameOverS.isPlaying)
        {
            GameOverS.Play();
        }
    }

    void PlayMagic()
    {
        AudioS.PlayOneShot(magic, 0.3f);
    }


    void PlayBossMusic()
    {
        MusicS.Stop();
        BossMusic.Play();
    }

    void StopBossMusic()
    {
        MusicS.Stop();
    }
}
