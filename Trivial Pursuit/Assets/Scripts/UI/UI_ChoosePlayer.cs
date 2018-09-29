using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChoosePlayer : UI_Window {

    public Text playerNameText;

    private void Awake()
    {
        StartCoroutine(AudioManager.instance.AudioFadeOut(AudioManager.instance.FindAudioClipByName("Start Screen Music"), 1));
    }

    private void OnEnable()
    {
        GameManager.instance.cannotPause = false;

        if (!GameManager.instance.cannotChooseNewPlayer)
        GameManager.instance.NextPlayer();

        playerNameText.text = GameManager.instance.players[GameManager.instance.currentPlayerIndex].playerName;
    }
}
