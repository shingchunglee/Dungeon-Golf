using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public bool PlayMusic = true;
  public AudioClip background;
  public AudioClip stroke;
  public AudioClip clubCollect;
  public AudioClip enemyDamage;
  public AudioClip playerDamage;

  [SerializeField] AudioSource musicSource;
  [SerializeField] AudioSource SFXSource;

  public static SoundManager Instance;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      // DontDestroyOnLoad(gameObject);
      PlayBackgroundMusic();
    }
    else if (Instance != this)
    {
      Destroy(gameObject);
    }
  }


  public void PlayBackgroundMusic()
  {
    if (musicSource == null) return;

    if (!PlayMusic) return;

    if (musicSource.clip != background)
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
