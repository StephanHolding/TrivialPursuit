using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestionScreen : UI_Window {

    string[] qAndA;

    [Header("UI References")]
    public Text questionText;
    public Text answerText;
    public Slider uiTimer;
    public Button correctAnswer;
    public Button incorrectAnswer;
    public Button showAnswerButton;

    private float timer;
    private bool startTimer;
    private bool startedTimerSound;
    private bool startedCymbalSwell;
    private bool startScreenMusicCheck;

    private void OnEnable()
    {
        correctAnswer.gameObject.SetActive(false);
        incorrectAnswer.gameObject.SetActive(false);
        showAnswerButton.gameObject.SetActive(true);

        qAndA = QuestionManager.instance.GetQandA();
        questionText.text = qAndA[0];

        timer = GameManager.instance.timerAmount;
        uiTimer.maxValue = timer;
        uiTimer.value = timer;

        startTimer = true;

        GameManager.instance.cannotPause = true;
        GameManager.instance.cannotChooseNewPlayer = false;

        if (!startScreenMusicCheck)
        {
            if (AudioManager.instance.IsBeingPlayed(AudioManager.instance.FindAudioClipByName("Start Screen Music")))
            {
                AudioManager.instance.StopAudio(AudioManager.instance.FindAudioClipByName("Start Screen Music"));
                startScreenMusicCheck = true;
            }
        }
            
        AudioManager.instance.PlayAudio(AudioManager.instance.GetRandomMusicClip(), true, "Music Volume");
    }

    private void Update()
    {
        if (GameManager.instance.debugMode)
        {
            if (Input.GetButtonDown("Debug Button"))
            {
                UIManager.instance.ShowMessage(QuestionManager.instance.GetCurrentCategory(), false);
            }
        }


        if (startTimer)
        {
            timer -= Time.deltaTime;
            uiTimer.value = timer;

            if (timer < 7.5f && startedTimerSound == false)
            {
                StartCoroutine(AudioManager.instance.AudioFadeIn(AudioManager.instance.FindAudioClipByName("Timer"), 7.5f, true, "SFX Volume"));
                startedTimerSound = true;
            }

            if (timer < 2.1f && startedCymbalSwell == false)
            {
                AudioManager.instance.PlayAudio(AudioManager.instance.FindAudioClipByName("Cymbal Swell"), false, "SFX Volume");
                startedCymbalSwell = true;
            }

            if (timer <= 0)
            {
                ShowAnswer();
            }
        }
    }

    public void ShowAnswer()
    {
        answerText.text = qAndA[1];
        startTimer = false;
        startedTimerSound = false;
        startedCymbalSwell = false;
        correctAnswer.gameObject.SetActive(true);
        incorrectAnswer.gameObject.SetActive(true);
        showAnswerButton.gameObject.SetActive(false);
        AudioManager.instance.StopAudio(AudioManager.instance.FindAudioClipByName("Timer"));
        AudioManager.instance.StopAudio(AudioManager.instance.chosenRandomMusic);
    }

    public void ShowAnswerEarly()
    {
        AudioManager.AudioClipData playedAudio = null;

        if (AudioManager.instance.IsBeingPlayed(playedAudio = AudioManager.instance.FindAudioClipByName("Cymbal Swell")))
            AudioManager.instance.StopAudio(playedAudio);

        if (AudioManager.instance.IsBeingPlayed(playedAudio = AudioManager.instance.FindAudioClipByName("Timer")))
            AudioManager.instance.StopAudio(playedAudio);

        AudioManager.instance.StopAudio(AudioManager.instance.chosenRandomMusic);

        AudioManager.instance.PlayAudio(AudioManager.instance.FindAudioClipByName("Cymbal No Swell"), false, "SFX Volume");

        answerText.text = qAndA[1];
        startTimer = false;
        startedTimerSound = false;
        startedCymbalSwell = false;
        correctAnswer.gameObject.SetActive(true);
        incorrectAnswer.gameObject.SetActive(true);
        showAnswerButton.gameObject.SetActive(false);
    }

    public void CorrectAnswer()
    {
        GameManager.instance.AddPointsToCurrentPlayer();
        answerText.text = "";
    }

    public void IncorrectAnswer()
    {
        answerText.text = "";
    }
}
