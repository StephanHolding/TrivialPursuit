using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionsMenu : UI_Window {

    [Header("Temporal settings")]
    public InputField directoryLink;
    public InputField questionTime;
    public InputField categoryAmount;
    public int playerOrder;
    public int categoryType;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    [Header("Other")]
    public Transform defaultActiveWindow;
    public Text playerOrderText;
    public Text categoryTypeText;
    public string[] playerOrderTypes;
    public string[] categoryTypes;

    private Transform activeWindow;

    private void OnEnable()
    {
        GameManager.instance.cannotPause = true;
        GetSettingsInfo();
    }

    public void ShowSubwindow(Transform windowToShow)
    {
        if (activeWindow == null)
            activeWindow = defaultActiveWindow;

        activeWindow.gameObject.SetActive(false);
        windowToShow.gameObject.SetActive(true);
        activeWindow = windowToShow;
    }

    public void SendToSettingsManager()
    {
        SettingsManager.instance.directoryLink = directoryLink.text;
        int.TryParse(questionTime.text, out SettingsManager.instance.questionTime);
        int.TryParse(categoryAmount.text, out SettingsManager.instance.categoryAmount);
        SettingsManager.instance.playerOrder = playerOrder;
        SettingsManager.instance.categoryType = categoryType;
        SettingsManager.instance.masterVolume = Mathf.RoundToInt(masterVolume.value);
        SettingsManager.instance.musicVolume = Mathf.RoundToInt(musicVolume.value);
        SettingsManager.instance.sfxVolume = Mathf.RoundToInt(sfxVolume.value);
    }

    public void GetSettingsInfo()
    {
        directoryLink.text = SettingsManager.instance.directoryLink;
        questionTime.text = SettingsManager.instance.questionTime.ToString();
        categoryAmount.text = SettingsManager.instance.categoryAmount.ToString();
        categoryType = SettingsManager.instance.categoryType;
        categoryTypeText.text = categoryTypes[categoryType];
        playerOrder = SettingsManager.instance.playerOrder;
        playerOrderText.text = playerOrderTypes[playerOrder];
        masterVolume.value = SettingsManager.instance.masterVolume;
        musicVolume.value = SettingsManager.instance.musicVolume;
        sfxVolume.value = SettingsManager.instance.sfxVolume;
    }

    public void NextPlayerOrder()
    {
        if (playerOrder + 1 < playerOrderTypes.Length)
        {
            playerOrder += 1;
        }
        else
        {
            playerOrder = 0;
        }

        playerOrderText.text = playerOrderTypes[playerOrder];
    }

    public void PreviousPlayerOrder()
    {
        if (playerOrder - 1 >= 0)
        {
            playerOrder -= 1;
        }
        else
        {
            playerOrder = playerOrderTypes.Length - 1;
        }

        playerOrderText.text = playerOrderTypes[playerOrder];
    }

    public void NextCategoryType()
    {
        if (categoryType + 1 < categoryTypes.Length)
        {
            categoryType += 1;
        }
        else
        {
            categoryType = 0;
        }

        categoryTypeText.text = categoryTypes[categoryType];
    }

    public void PreviousCategoryType()
    {
        if (categoryType - 1 >= 0)
        {
            categoryType -= 1;
        }
        else
        {
            categoryType = categoryTypes.Length - 1;
        }

        categoryTypeText.text = categoryTypes[categoryType];
    }
}
