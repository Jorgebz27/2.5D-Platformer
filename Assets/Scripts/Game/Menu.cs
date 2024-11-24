using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject config;
    public GameObject audio;
    public GameObject control;
    
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        config.SetActive(true);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void BacktoMain()
    {
        config.SetActive(false);
    }

    public void Back()
    {
        config.SetActive(true);
        audio.SetActive(false);
        control.SetActive(false);
    }
    
    public void Audio()
    {
        audio.SetActive(true);
        config.SetActive(false);
    }

    public void Control()
    {
        control.SetActive(true);
        config.SetActive(false);
    }
}
