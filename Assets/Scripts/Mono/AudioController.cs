using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource sfx;
    [SerializeField]
    private AudioSource sfxSmall;

    [Header("Music")] 
    public AudioClip musicBackground;
    
    [Header("SFX")]
    public AudioClip button;
    public AudioClip fail;
    public AudioClip sell;
    public AudioClip buy;
    
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        
        if (instance == null) instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        music.clip = musicBackground;
        music.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfx.PlayOneShot(audioClip);
    }
    
    public void PlaySFXSmall(AudioClip audioClip)
    {
        sfxSmall.PlayOneShot(audioClip);
    }
}
