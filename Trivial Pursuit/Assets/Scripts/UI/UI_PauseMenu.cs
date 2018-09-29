using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseMenu : UI_Window {

    public Transform scoreParent;
    public GameObject playerNamePrefab;

    private GameManager.Players[] sortedPlayers;
    public List<GameObject> instantiatedObjects;

    private void OnEnable()
    {
        ShowScore();
        GameManager.instance.cannotPause = true;
    }

    public void ShowScore()
    {
        sortedPlayers = GameManager.instance.SortPlayersToScore();
        ClearPlayerList();

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            GameObject newObject = Instantiate(playerNamePrefab, scoreParent.position, Quaternion.identity);
            newObject.transform.SetParent(scoreParent, false);
            instantiatedObjects.Add(newObject);

            Text playerName = newObject.transform.GetChild(0).GetComponent<Text>();
            playerName.text = sortedPlayers[i].playerName;

            Button b = playerName.transform.GetChild(0).GetComponent<Button>();
            b.interactable = false;
            ColorBlock cb = b.colors;
            cb.disabledColor = new Color(1, 1, 1, 1);
            b.colors = cb;

            Text score = b.transform.GetChild(0).GetComponent<Text>();
            score.text = sortedPlayers[i].playerScore.ToString();
        }
    }

    public void ClearPlayerList()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }

        instantiatedObjects.Clear();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
