using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [System.Serializable]
    public class Players
    {
        public string playerName;
        public int playerScore;
    }

    [Header("Game Flow")]
    public List<Players> players = new List<Players>();
    public UI_Window pauseMenu; 

    public bool cannotPause;
    public bool cannotChooseNewPlayer;
    public int currentPlayerIndex;
    public int timerAmount;
    public int playerOrder;

    [Header("Debug Mode")]
    public bool debugMode;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timerAmount = SettingsManager.instance.questionTime;
        playerOrder = SettingsManager.instance.playerOrder;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!cannotPause)
            {
                UIManager.instance.RememberLastWindow();
                UIManager.instance.ShowWindow(pauseMenu);
            }
        }
    }

    public void AddPlayer(string name)
    {
        players.Add(new Players() { playerName = name, playerScore = 0 });
    }

    public void RemovePlayer(string name)
    {
        players.Remove(FindPlayer(name));
    }

    public Players FindPlayer(string name)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerName == name)
            {
                return players[i];
            }
        }
        
        return null;
    }

    public Players[] SortPlayersToScore()
    {
        List<Players> toReturn = new List<Players>();
        List<Players> originalList = new List<Players>(players);

        while (originalList.Count != 0)
        {
            Players currentHighest = originalList[0];
            for (int i = 0; i < originalList.Count; i++)
            {
                if (originalList[i].playerScore > currentHighest.playerScore)
                {
                    currentHighest = originalList[i];
                }
            }

            toReturn.Add(currentHighest);
            originalList.Remove(currentHighest);
        }

        return toReturn.ToArray();
    }

    public void NextPlayer()
    {
        switch (playerOrder)
        {
            case 0:
                if (currentPlayerIndex + 1 < players.Count)
                {
                    currentPlayerIndex += 1;
                }
                else
                {
                    currentPlayerIndex = 0;
                }
                break;
            case 1:
                currentPlayerIndex = Random.Range(0, players.Count);
                break;
        }

        cannotChooseNewPlayer = true;
    }

    public void AddPointsToCurrentPlayer()
    {
        players[currentPlayerIndex].playerScore += 1;
    }
}
