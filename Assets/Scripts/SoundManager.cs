using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool PlayMusic = true;
    public AudioClip ThemeMusic;
    public AudioClip DungeonMusic;
    public AudioClip stroke;
    public AudioClip clubCollect;
    public AudioClip enemyDamage;
    public AudioClip playerDamage;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource musicSource2;
    [SerializeField] AudioSource SFXSource;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayThemeMusic();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void PlayThemeMusic()
    {
        if (musicSource == null || ThemeMusic == null) return;

        if (!PlayMusic) return;

        if (musicSource.clip != ThemeMusic)
        {
            musicSource.clip = ThemeMusic;
            musicSource.loop = false;
            musicSource.Play();
            StartCoroutine(WaitForThemeMusicToEnd());
        }
    }

    private IEnumerator WaitForThemeMusicToEnd()
    {
        yield return new WaitForSecondsRealtime(105);

        PlayDungeonMusic();
    }

    private void PlayDungeonMusic()
    {
        if (musicSource2 == null || DungeonMusic == null) return;

        musicSource2.clip = DungeonMusic;
        musicSource2.loop = true;
        musicSource2.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
