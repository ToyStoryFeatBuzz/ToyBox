using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuEventsManager : MonoBehaviour
{
    public static MenuEventsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Time.timeScale = 1.0f;
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void PlayScene(string name)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name);
    }

    public void PauseScene()
    {
        Time.timeScale = 0f;
    }
    public void UnPauseScene()
    {
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(currentScene.name);
    }

    public void ChangeMenuSection(MenuSection menusSections)
    {
        foreach (GameObject menuSection in menusSections._menuSectionToDisabled)
        {
            menuSection.SetActive(false);
        }
        foreach (GameObject menuSection in menusSections._menuSectionToActivated)
        {
            menuSection.SetActive(true);
        }
    }
}
