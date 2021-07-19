using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    //Skripta za main menu

    public GameObject mainMenuPanel,settingsPanel;
    public AudioSource soundEffect;
    public bool isSoundActive;

    public Dropdown rezolucijeDD;


    [Range (0.0f , 1.0f)]
    public float soundVolume;

    public Slider volumeSlider;
    public Toggle soundFxCheckBox;


    public InputField playerName;

    private void Start()
    {
        //Start funkcija koja vuce sve vrednosti iz Player Preferences 
        //i postavlja na UI elemente iste vrednosti

        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        if (PlayerPrefs.GetInt("soundOn") == 1)
        {
            soundFxCheckBox.isOn = true;
        }
        else if (PlayerPrefs.GetInt("soundOn") == 0)
        {
            soundFxCheckBox.isOn = false;
        }
    }
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
        else {
            PlayerPrefs.SetInt("soundOn", 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();




    }
    //Handler za play dugme
    public void Play() {
        SceneManager.LoadScene("GameScene");

        if (!playerName.text.Equals(null))
        {
            PlayerPrefs.SetString("playerName", playerName.text);
        }

        if (isSoundActive)
        {
            soundEffect.Play();
        }
    }
    //Handler za settings dugme 
    public void Settings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false); 
        if (isSoundActive)
        {
            soundEffect.Play();
        }
    }
    //Handler za goBack dugme
    public void goBack() {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        if (isSoundActive)
        {
            soundEffect.Play();
        }
    }


    //Handler za exit dugme
    public void Exit() {
        Application.Quit(0); 
        if (isSoundActive)
        {
            soundEffect.Play();
        }
    }
}
