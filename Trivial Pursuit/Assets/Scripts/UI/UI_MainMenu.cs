using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : UI_Window {

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.FindAudioClipByName("Start Screen Music"), true, "Music Volume");
    }

    private void OnEnable()
    {
        GameManager.instance.cannotPause = true;

    }
}
