using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public float musicVolume = 10f;
    public float soundVolume = 10f;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private Settings settings;

    private void Start()
    {
        settings = FindObjectOfType<Settings>();
        settings.LoadIntoSettingsMenu();
    }

    public void LoadSettings(float givenMusicVolume, float givenSoundVolume)
    {
        musicVolume = givenMusicVolume;
        soundVolume = givenSoundVolume;

        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

    public void SaveSettings()
    {
        settings.SaveSettings(musicSlider.value, soundSlider.value);
    }
}
