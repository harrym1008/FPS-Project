using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI musicPercent;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;



    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} × {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && 
                resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void SetVolume(float volume)
    {
        float decibels = volume * 0.30f - 30f;
        musicPercent.text = $"{volume}%";

        if (volume == 0f)
            audioMixer.SetFloat("MasterVolume", -80f);
        else
            audioMixer.SetFloat("MasterVolume", decibels);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVSYNC(bool isVSYNC)
    {
        QualitySettings.vSyncCount = isVSYNC ? 1 : 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
