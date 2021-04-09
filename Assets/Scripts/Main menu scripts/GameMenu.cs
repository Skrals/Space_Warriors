using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour {
    public GameObject Game_menu;
    float timer = 1;
	void Update ()
    {
        //////Пауза и открытие меню/////
        Time.timeScale = timer;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Game_menu.SetActive(!Game_menu.activeSelf);
            if (Game_menu.activeSelf == true)
            {
                timer = 0;
            }
            else
            {
                timer = 1;
            }
        }
        //////Пауза и открытие меню/////
    }
    public void Continue()
    {
        Game_menu.SetActive(!Game_menu.activeSelf);
        timer = 1;
    }
    public void ExMainMenu()
    {
        Application.LoadLevel(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
