using UnityEngine;

public class Settings : MonoBehaviour
{
    public float musicVolume = 10f;
    public float soundVolume = 10f;

    public void LoadIntoGame()
    {
        Main main = FindObjectOfType<Main>();

        main.musicVolume = musicVolume;
        main.soundVolume = soundVolume;
    }

    public void LoadIntoSettingsMenu()
    {
        FindObjectOfType<SettingsMenu>().LoadSettings(musicVolume, soundVolume);
    }

    public void SaveSettings(float givenMusicVolume, float givenSoundVolume)
    {
        musicVolume = givenMusicVolume;
        soundVolume = givenSoundVolume;
    }
}
