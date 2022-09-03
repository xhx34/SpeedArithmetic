using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioSource audioMusic;
    public AudioSource audioSound;
    public Musics musics;
    public Sounds sounds;
    public Slider musicSlider;
    public Slider soundSlider;
    [HideInInspector]
    public float musicVolume;
    [HideInInspector]
    public float soundVolume;

    private void Awake()
    {
        instance = this;
        musicVolume = ES3.Load("musicVolume", "Game.es3", 1f);
        soundVolume = ES3.Load("soundVolume", "Game.es3", 1f);
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        musicSlider.onValueChanged.AddListener((float value) => SetMusicVolumn());
        soundSlider.onValueChanged.AddListener((float value) => SetSoundVolumn());
    }

    public void PlayMusic(int id)
    {
        audioMusic.PlayOneShot(musics.musics[id]);
        audioMusic.volume = musicVolume;
    }

    public void PlaySound(int id)
    {
        audioSound.PlayOneShot(sounds.sounds[id]);
        audioSound.volume = soundVolume;
    }

    public void PlaySound(AudioSource source, int id)
    {
        source.PlayOneShot(sounds.sounds[id]);
        source.volume = soundVolume;
    }

    public void SetMusicVolumn()
    {
        musicVolume = musicSlider.value;
        audioMusic.volume = musicVolume;
    }

    public void SetSoundVolumn()
    {
        soundVolume = soundSlider.value;
        audioSound.volume = soundVolume;
    }

    public void SaveVolumn()
    {
        ES3.Save("soundVolume", soundVolume, "Game.es3");
        ES3.Save("musicVolume", musicVolume, "Game.es3");
    }
}
