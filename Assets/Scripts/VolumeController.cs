using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixerMusic;
    public AudioMixer audioMixerSFX;

    private void Start()
    {
        float playerPrefMusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVolume", 1));
        audioMixerMusic.SetFloat("MusicVol", (1 - Mathf.Sqrt(playerPrefMusicVolume)) * -80f);

        float playerPrefSfxVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("SfxVolume", 1));
        audioMixerSFX.SetFloat("MusicVol", (1 - Mathf.Sqrt(playerPrefSfxVolume)) * -80f);
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioMixerMusic.SetFloat("MusicVol", (1 - Mathf.Sqrt(volume)) * -80f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioMixerSFX.SetFloat("MusicVol", (1 - Mathf.Sqrt(volume)) * -80f);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }
}