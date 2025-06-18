using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> soundEffects;
    public List<AudioSource> music;

    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource backgroundMusic;
    public AudioSource clickSound;

    public void LoadVolumeSettings(float givenMusicVolume, float givenSoundVolume)
    {
        foreach (AudioSource sound in soundEffects)
            sound.volume = givenSoundVolume;

        foreach (AudioSource sound in music)
            sound.volume = givenMusicVolume;
    }
}
