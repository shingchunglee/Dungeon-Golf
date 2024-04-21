using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  [SerializeField] AudioSource musicSource;
  [SerializeField] AudioSource SFXSource;

  public AudioClip background;
  public AudioClip stroke;
  public AudioClip clubCollect;

  
    void Awake()
    {
        DontDestroyOnLoad(gameObject); 
        PlayBackgroundMusic(); 
    }

    public void PlayBackgroundMusic()
    {
        if(musicSource != null && background != null)
        {
            musicSource.clip = background; 
            musicSource.loop = true; 
            musicSource.Play(); 
        }
    }



  public void PlaySFX(AudioClip clip)
  {
    SFXSource.PlayOneShot(clip);
  }



    


}
