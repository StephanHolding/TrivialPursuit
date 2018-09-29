using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    [Header("UI References")]
    public Transform canvas;
    public Text messageText;

    public UI_Window activeWindow;
    public UI_Window rememberedWindow;

    [Header("Normal Button Proporties")]
    public ColorBlock normalButtonColors;

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

        UI_Window[] activeWindows = canvas.GetComponentsInChildren<UI_Window>(false);
        if (activeWindows.Length > 1 || activeWindows.Length == 0)
        {
            Debug.LogError("There is either no window or to many windows active");
        }
        else
        {
            activeWindow = activeWindows[0];
        }

    }

    private void Start()
    {
        MatchButtonColor("Changeable Colors", normalButtonColors);
    }

    public void ShowWindow(UI_Window toShow)
    {
        if (activeWindow != null)
            activeWindow.gameObject.SetActive(false);
        toShow.gameObject.SetActive(true);
        activeWindow = toShow;
    }

    public void RememberLastWindow()
    {
        rememberedWindow = activeWindow;
    }

    public void LoadRememberedWindow()
    {
        if (activeWindow != null)
            activeWindow.gameObject.SetActive(false);
        rememberedWindow.gameObject.SetActive(true);
        activeWindow = rememberedWindow;
        rememberedWindow = null;
    }

    public void ShowPreviousWindow()
    {
        UI_Window toShow = rememberedWindow;

        if (activeWindow != null)
            activeWindow.gameObject.SetActive(false);
        toShow.gameObject.SetActive(true);
        activeWindow = toShow;
    }

    public void ShowMessage(string message, bool fatal)
    {
        Animator anim = messageText.transform.parent.GetComponent<Animator>();

        if (!anim.GetBool("Return"))
        {
            messageText.text = message;

            if (!fatal)
            {
                anim.SetTrigger("ShowMessage");
                anim.SetTrigger("Return");
            }
            else
            {
                anim.SetTrigger("ShowFatalMessage");
            }

        }
    }

    private void MatchButtonColor(string tag, ColorBlock cb)
    {
        Button[] allButtons = canvas.GetComponentsInChildren<Button>(true);
        InputField[] allInputfields = canvas.GetComponentsInChildren<InputField>(true);

        foreach (Button b in allButtons)
        {
            if (b.gameObject.tag == tag)
            {
                b.colors = cb;
            }
        }

        foreach(InputField i in allInputfields)
        {
            if (i.gameObject.tag == tag)
            {
                i.colors = cb;
            }
        }
    }
}
