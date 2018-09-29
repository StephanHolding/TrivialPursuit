using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterScreen : UI_Window {

    [Header("UI References")]
    public Transform participantsParent;
    public InputField inputField;

    [Header("Prefabs")]
    public GameObject playerNameButton;

    private void OnEnable()
    {
        GameManager.instance.cannotPause = false;
    }

    public void AddNewPlayer()
    {
        if (GameManager.instance.players.Count >= 10)
        {
            UIManager.instance.ShowMessage("Er kunnen maximaal 10 spelers meedoen.", false);
            return;
        }

        string playerName = null;
        if (inputField.text != "")
        {
            playerName = inputField.text;
        }
        else
        {
            UIManager.instance.ShowMessage("De naam mag niet leeg zijn.", false);
            return;
        }

        if (GameManager.instance.FindPlayer(playerName) == null)
        {
            GameManager.instance.AddPlayer(playerName);
        }
        else
        {
            UIManager.instance.ShowMessage("Er is al een speler met de naam " + playerName, false);
            return;
        }

        GameObject newButton = Instantiate(playerNameButton, participantsParent.position, Quaternion.identity);
        newButton.transform.SetParent(participantsParent, false);

        Text buttonName = newButton.transform.GetChild(0).GetComponent<Text>();
        buttonName.text = playerName;

        Button b = buttonName.transform.GetChild(0).GetComponent<Button>();
        b.onClick.AddListener(delegate { RemovePlayer(newButton, playerName); });

        inputField.text = "";
    }

    public void RemovePlayer(GameObject toDestroy, string playerName)
    {
        Destroy(toDestroy);

        GameManager.instance.RemovePlayer(playerName);
    }

    public void StartGame(UI_Window windowToShow)
    {
        if (GameManager.instance.players.Count > 1)
        {
            UIManager.instance.ShowWindow(windowToShow);
        }
        else
        {
            UIManager.instance.ShowMessage("Er moeten minstens 2 spelers zijn om het spel te beginnen.", false);
        }
    }

    public void SubmitText()
    {
        if (Input.GetButtonDown("Submit"))
        {
            AddNewPlayer();
        }
    }
}
