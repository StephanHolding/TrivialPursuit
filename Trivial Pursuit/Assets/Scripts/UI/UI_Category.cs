using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Category : UI_Window {

    [Header("UI References")]
    public Transform allCategoriesWindow;
    public Transform pickedCategoriesWindow;

    [Header("Other")]
    public GameObject categoryButtonPrefab;
    public UI_Window questionPanel;

    private bool cannotChange;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    private void OnEnable()
    {
        GameManager.instance.cannotPause = false;

        switch (QuestionManager.instance.categoryType)
        {
            case 0:
                allCategoriesWindow.gameObject.SetActive(true);
                pickedCategoriesWindow.gameObject.SetActive(false);
                break;
            case 1:
                allCategoriesWindow.gameObject.SetActive(false);
                pickedCategoriesWindow.gameObject.SetActive(true);
                ShowChosenCategories();
                break;
        }
    }

    public void SetCategory(string category)
    {
        cannotChange = false;

        if (QuestionManager.instance.FindCategory(category).questions.Count == 0)
        {
            UIManager.instance.ShowMessage("Er zijn geen vragen meer in deze categorie.", false);
            cannotChange = true;
            return;
        }
        QuestionManager.instance.SetCategory(category);
    }

    public void StartGame(UI_Window toShow)
    {
        if(!cannotChange)
        {
            UIManager.instance.ShowWindow(toShow);
        }
    }

    private void ShowChosenCategories()
    {
        List<string> chosenCategories = new List<string>(QuestionManager.instance.PickCategories());

        foreach(GameObject g in spawnedButtons)
        {
            Destroy(g);
        }

        spawnedButtons.Clear();

        for (int i = 0; i < chosenCategories.Count; i++)
        {
            int index = i;

            GameObject newObject = Instantiate(categoryButtonPrefab, pickedCategoriesWindow.position, Quaternion.identity);
            newObject.transform.SetParent(pickedCategoriesWindow, false);

            Button b = newObject.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(delegate { SetCategory(chosenCategories[index]); });
            b.onClick.AddListener(delegate { StartGame(questionPanel); });

            Text t = b.transform.GetChild(0).GetComponent<Text>();
            t.text = chosenCategories[i];

            spawnedButtons.Add(newObject);
        }
    }
}
