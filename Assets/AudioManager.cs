using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------ Audio Source --------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("------------ Audio Clip --------------")]
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;
    public AudioClip enemyExplosion;
    public AudioClip playerExplosion;
    public AudioClip playerShoot;
    public AudioClip powerup;
    public AudioClip bossHit;



    // Start is called before the first frame update
    void Start()
    {
        MusicToRegular();
        
    }

    public void MusicToBoss()
    {
        musicSource.clip = bossMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void MusicToRegular()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    
}
