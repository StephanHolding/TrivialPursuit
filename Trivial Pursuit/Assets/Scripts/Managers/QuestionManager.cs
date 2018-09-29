using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour {

    public static QuestionManager instance;

    [System.Serializable]
    public class QaAs
    {
        public string category;
        public List<string> questions = new List<string>();
        public List<string> answers = new List<string>();
    }

    public QaAs[] qAndAs;

    public int categoryType;
    public int categoryAmount;

    private int qAndAsIndex;
    private int currentQuestionIndex;

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
    }

    private void Start()
    {
        LoadQuestions();
    }

    public void LoadQuestions()
    {
        for (int i = 0; i < qAndAs.Length; i++)
        {
            string[] qArray = DataManager.instance.LoadStringFile(qAndAs[i].category + " Questions.txt");
            string[] aArray = DataManager.instance.LoadStringFile(qAndAs[i].category + " Answers.txt");

            for (int q = 0; q < qArray.Length; q++)
            {
                qAndAs[i].questions.Add(qArray[q]);
                qAndAs[i].answers.Add(aArray[q]);
            }
        }
    }

    public void SetCategory(string changeTo)
    {
        for (int i = 0; i < qAndAs.Length; i++)
        {
            if (qAndAs[i].category == changeTo)
            {
                qAndAsIndex = i;
            }
        }
    }

    public string GetCurrentCategory()
    {
        string toReturn = "Current Q and A are at category " + qAndAs[qAndAsIndex].category + ", at index " + currentQuestionIndex.ToString();

        return toReturn;
    }

    public string[] GetQandA()
    {
        if (qAndAs[qAndAsIndex].questions.Count == 0 || qAndAs[qAndAsIndex].questions.Count == 0)
        {
            Debug.LogError("There are no more questions left in category: " + qAndAs[qAndAsIndex].category);
            return null;
        }

        if (qAndAs[qAndAsIndex].questions.Count != qAndAs[qAndAsIndex].answers.Count)
        {
            Debug.LogError("The length of the Questions and Answers array of category: " + qAndAs[qAndAsIndex].category + " is not the same.");
            return null;
        }

        List<string> toReturn = new List<string>();

        int index = Random.Range(0, qAndAs[qAndAsIndex].questions.Count);

        toReturn.Add(qAndAs[qAndAsIndex].questions[index]);
        toReturn.Add(qAndAs[qAndAsIndex].answers[index]);

        qAndAs[qAndAsIndex].questions.RemoveAt(index);
        qAndAs[qAndAsIndex].answers.RemoveAt(index);

        currentQuestionIndex = index;
        return toReturn.ToArray();
    }

    public QaAs FindCategory(string categoryName)
    {
        for (int i = 0; i < qAndAs.Length; i++)
        {
            if (qAndAs[i].category == categoryName)
            {
                return qAndAs[i];
            }
        }

        return null;
    }

    public List<string> PickCategories()
    {
        List<string> pickedCategories = new List<string>();

        for (int i = 0; i < categoryAmount; i++)
        {
            int toPick = Random.Range(0, qAndAs.Length);

            if (qAndAs[toPick].questions.Count > 0)
            {
                pickedCategories.Add(qAndAs[toPick].category);
            }
            else if (qAndAs[toPick].questions.Count == 0)
            {
                if (toPick + 1 >= qAndAs.Length)
                {
                    pickedCategories.Add(qAndAs[toPick - 1].category);
                }
                else
                {
                    pickedCategories.Add(qAndAs[toPick + 1].category);
                }
            }
        }

        return pickedCategories;
    }
}
