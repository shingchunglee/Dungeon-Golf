using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public VolumeController volumeController;
    public bool PlayMusic = true;
    public AudioClip ThemeMusic;
    public AudioClip DungeonMusic;
    public AudioClip LavaLevelMusic;
    public AudioClip stroke;
    public AudioClip clubCollect;
    public AudioClip enemyDamage;
    public AudioClip playerDamage;
    public AudioClip levelUp;

    public float LavaVolumeLevel = 0.089f;
    public float ThemeVolumeLevel = 0.114f;
    public float DungeonVolumeLevel = 0.089f;

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
        isLavaMusicPlaying = false;

        if (musicSource == null || ThemeMusic == null) return;

        if (!PlayMusic) return;

        if (musicSource.clip != ThemeMusic)
        {
            musicSource.clip = ThemeMusic;
            musicSource.loop = false;
            musicSource.volume = ThemeVolumeLevel;
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
        musicSource2.volume = DungeonVolumeLevel;
        musicSource2.Play();
    }

    private bool isLavaMusicPlaying = false;
    public void FadeInLavaMusic()
    {
        isLavaMusicPlaying = true;
        StartCoroutine(FadeOutAndFadeInLavaMusic());
    }

    private IEnumerator FadeOutAndFadeInLavaMusic()
    {

        float duration = 6f;
        float targetVolume = 0f;
        float currentTime = 0;

        float start = musicSource2.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicSource2.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return new WaitForEndOfFrame();
            Debug.Log("waiting...");
        }

        Debug.Log("waiting Done!");
        musicSource.clip = LavaLevelMusic;
        musicSource.loop = true;
        musicSource.volume = LavaVolumeLevel;
        musicSource.Play();

        yield break;
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
