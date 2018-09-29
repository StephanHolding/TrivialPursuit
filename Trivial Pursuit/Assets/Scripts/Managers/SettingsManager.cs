using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    public static SettingsManager instance;

    [Header("Saveable settings")]
    public string directoryLink;
    public int questionTime;
    public int playerOrder;
    public int categoryType;
    public int categoryAmount;
    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();

    }

    private void Start()
    {
        ApplySettings();
    }


    public void SaveSettings()
    {
        PlayerPrefs.SetString("DirectoryLink", directoryLink);
        PlayerPrefs.SetInt("QuestionTime", questionTime);
        PlayerPrefs.SetInt("PlayerOrder", playerOrder);
        PlayerPrefs.SetInt("CategoryType", categoryType);
        PlayerPrefs.SetInt("CategoryAmount", categoryAmount);
        PlayerPrefs.SetInt("MasterVolume", masterVolume);
        PlayerPrefs.SetInt("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("SFXVolume", sfxVolume);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("DirectoryLink"))
        {
            directoryLink = PlayerPrefs.GetString("DirectoryLink");
            questionTime = PlayerPrefs.GetInt("QuestionTime");
            playerOrder = PlayerPrefs.GetInt("PlayerOrder");
            categoryType = PlayerPrefs.GetInt("CategoryType");
            categoryAmount = PlayerPrefs.GetInt("CategoryAmount");
            masterVolume = PlayerPrefs.GetInt("MasterVolume");
            musicVolume = PlayerPrefs.GetInt("MusicVolume");
            sfxVolume = PlayerPrefs.GetInt("SFXVolume");
        }
    }

    public void ApplySettings()
    {
        DataManager.instance.directory = directoryLink;
        GameManager.instance.timerAmount = questionTime;
        GameManager.instance.playerOrder = playerOrder;
        QuestionManager.instance.categoryType = categoryType;
        QuestionManager.instance.categoryAmount = categoryAmount;
        AudioManager.instance.ApplyVolumes();
    }

}
