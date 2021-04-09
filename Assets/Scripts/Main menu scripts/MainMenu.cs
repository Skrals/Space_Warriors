using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volume;
    public GameObject settings;
    public GameObject modes;
    void Update()
    {
        AudioListener.volume = volume.value;
        SetSound();

    }
    public void SinglePlayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {

    }

    public void Modes()
    {
        if (settings.activeSelf == false)
            modes.SetActive(!modes.activeSelf);
        else
        {
            settings.SetActive(!settings.activeSelf);
            modes.SetActive(!modes.activeSelf);
        }
    }

    public void Settings()
    {
        if(modes.activeSelf == false)
           settings.SetActive(!settings.activeSelf);
        else
        {
            modes.SetActive(!modes.activeSelf);
            settings.SetActive(!settings.activeSelf);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSound()
    {
        Global.sound = volume.value;
    }
}
