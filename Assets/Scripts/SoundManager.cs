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

  public void PlaySFX(AudioClip clip)
  {
    SFXSource.PlayOneShot(clip);
  }







}
