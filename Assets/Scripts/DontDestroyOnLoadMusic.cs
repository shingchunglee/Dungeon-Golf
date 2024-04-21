using UnityEngine;
using UnityEngine.Audio;

public class DontDestroyMusic : MonoBehaviour
{
    private GameObject musicPlayer;
    public AudioClip SoundClip;
    public AudioMixerGroup audioMixerGroup;
    public AudioMixer audioMixer;
    void Start()
    {
        //When the scene loads it checks if there is an object called "MUSIC".
        musicPlayer = GameObject.FindWithTag("Music");
        if (musicPlayer == null)
        {
            musicPlayer = gameObject;
            musicPlayer.tag = "Music";
            DontDestroyOnLoad(musicPlayer);
            AudioSource soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.loop = true;
            soundSource.clip = SoundClip;
            soundSource.Play();
            soundSource.outputAudioMixerGroup = audioMixerGroup;

            int playerPrefVolume = PlayerPrefs.GetInt("MusicVolume", 9);
            float vol = playerPrefVolume / 9f;
            audioMixer.SetFloat("MusicVol", (1 - Mathf.Sqrt(vol)) * -80f);
        }
    }
}
