using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Skripta za glavni game menadzer

    [Header("Settings")]
    public bool isSoundActive;
    public Dropdown rezolucijeDD;
    [Range(0.0f, 1.0f)]
    public float soundVolume;
    public Slider volumeSlider;
    public Toggle soundFxCheckBox;

    public TextMeshProUGUI playerName;

    public GameObject pauseScreen, pauseBtn, settingsScreen;

    public AudioSource soundEffect;
    public AudioSource music;

    private void Start()
    {
        if (PlayerPrefs.GetString("playerName") == "")
        {
            playerName.text = "Blue Tower";
        }
        else
        {
            playerName.text = PlayerPrefs.GetString("playerName");
        }

        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        pauseScreen.transform.localScale = Vector2.zero;
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundFxCheckBox.isOn = true;
        }
        else if (PlayerPrefs.GetInt("soundOn") == 0)
        {
            soundFxCheckBox.isOn = false;
        }
    }

    //uzima Preference koji je postavljen u settings ekran i postavlja jacinu ambijentalne muzike
    private void Update()
    {
        switch (rezolucijeDD.value)
        {
            case 0:
                Screen.SetResolution(1280, 664, false, 60);
                break;
            case 1:
                Screen.SetResolution(1366, 768, false, 60);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, false, 60);
                break;
        }


        soundVolume = PlayerPrefs.GetFloat("musicVolume");

        isSoundActive = soundFxCheckBox.isOn;

        if (soundFxCheckBox.isOn == true)
        {
            PlayerPrefs.SetInt("soundOn", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("soundOn", 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();




        music.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    //globalni boolean za pauziranje igre
    public bool isPaused;
    public bool isSettings;
    //Handler za pause dugme
    public void Pause()
    {
        isPaused = true;
        pauseBtn.SetActive(false);
        pauseScreen.transform.LeanScale(Vector2.one, 0.8f);

        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }
    //Handler za resume dugme
    public void Resume()
    {
        isPaused = false;
        pauseBtn.SetActive(true);
        pauseScreen.transform.LeanScale(Vector2.zero, 0.8f);
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }
    public void SettingsInGame() {
        isSettings = true;
        settingsScreen.SetActive(true);
        pauseScreen.SetActive(false);
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }

    public void closeSettings() {
        isSettings = false;
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }

    //Handler za MainMenu dugme
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }
    //Handler za MainMenu dugme
    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundEffect.Play();
        }
    }
}
