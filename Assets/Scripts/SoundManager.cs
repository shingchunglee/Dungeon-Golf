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
    public AudioClip levelUp;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource musicSource2;
    [SerializeField] AudioSource SFXSource;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            this.transform.SetParent(null);
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
        yield return new WaitForSeconds(98);

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

    public void PlayLevelUpSFX()
    {
        StartCoroutine(LevelUpSFXDelay());
    }

    private IEnumerator LevelUpSFXDelay()
    {
        yield return new WaitForSeconds(0.3f);
        PlaySFX(levelUp);
    }
}
