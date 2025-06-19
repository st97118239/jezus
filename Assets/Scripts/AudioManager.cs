using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> music;
    public List<AudioSource> soundEffects;

    public AudioSource backgroundMusic;
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource clickSound;
    public AudioSource castleBellSound;
    public AudioSource DestinationChosenSound;

    public float musicVolume;
    public float soundVolume;

    public void LoadVolumeSettings(float givenMusicVolume, float givenSoundVolume)
    {
        musicVolume = givenMusicVolume;
        soundVolume = givenSoundVolume;

        foreach (AudioSource sound in music)
            sound.volume = musicVolume;

        foreach (AudioSource sound in soundEffects)
            sound.volume = soundVolume;
    }
}
