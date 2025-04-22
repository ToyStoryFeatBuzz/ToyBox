using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuEventsManager : MonoBehaviour
{
    public static MenuEventsManager Instance;
    [SerializeField] private TMP_Dropdown _dropdownResolution;
    private Resolution[] _resolutions;
    [SerializeField] private Toggle _toggleFullScreen;

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
        //Screen.SetResolution(1920, 1080, true);
        Time.timeScale = 1.0f;
        GetResolution();
        
        if (_toggleFullScreen != null)
            _toggleFullScreen.isOn = Screen.fullScreen;
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    private void GetResolution()
    {
        _resolutions = Screen.resolutions.Select(resolutions => new Resolution { width = resolutions.width, height = resolutions.height }).Distinct().ToArray();
        _dropdownResolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolution = i;
            }
        }
        _dropdownResolution.AddOptions(options);
        _dropdownResolution.value = currentResolution;
        _dropdownResolution.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void PlayScene(string name)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name);
        if (AudioManager.Instance.IsSoundInList(AudioManager.Instance._musicSounds, name))
        {
            AudioManager.Instance.PlayMusic(name);
        }
        else
        {
            AudioManager.Instance.StopMusic();
        }
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
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
