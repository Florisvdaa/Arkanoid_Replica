using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [Header("Main Menu Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button quitYesButton;
    [SerializeField] private Button quitNoButton;
    [SerializeField] private Button exitCreditsButton;
    [SerializeField] private Button exitSettingsButton;

    [Header("Screens")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingScreen;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject quitScreen;

    [SerializeField] private VideoPlayer mainVideoPlayer;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player mainMenuOpenFeedback;
    [SerializeField] private MMF_Player mainMenuCloseFeedback;
    [SerializeField] private MMF_Player mainMenuOpenAgainFeedback; // plays without the button fade in.
    [SerializeField] private MMF_Player settingsMenuOpenFeedback;
    [SerializeField] private MMF_Player settingsMenuCloseFeedback;
    [SerializeField] private MMF_Player creditsMenuOpenFeedback;
    [SerializeField] private MMF_Player creditsMenuCloseFeedback;
    [SerializeField] private MMF_Player exitMenuOpenFeedback;
    [SerializeField] private MMF_Player exitMenuCloseFeedback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Main menu buttons
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        quitButton.onClick.AddListener(OpenExitCheck);

        // Options screen
        exitSettingsButton.onClick.AddListener(CloseSettings);

        // Credits screen
        exitCreditsButton.onClick.AddListener(CloseCredits);

        // Quit confirmation
        quitYesButton.onClick.AddListener(ExitGame);
        quitNoButton.onClick.AddListener(CloseExitCheck);

        // Ensure all screens start hidden
        settingScreen.SetActive(true);
        creditsScreen.SetActive(true);
        quitScreen.SetActive(true);
        mainMenuScreen.SetActive(true);

        mainVideoPlayer.Play();

        mainVideoPlayer.loopPointReached += OnMainMenuVideoFinished;
    }

    private void OnMainMenuVideoFinished(VideoPlayer vp)
    {
        //mainMenuAnimator.SetTrigger("ShowMenu");
        Debug.Log("Starting video done");
        mainMenuOpenFeedback.PlayFeedbacks();
    }

    private void StartGame()
    {
        Debug.Log("Start Game!");
        // SceneManager.LoadScene("GameScene");
    }

    private void OpenSettings()
    {
        //settingScreen.SetActive(true);
        mainMenuCloseFeedback.PlayFeedbacks();

        settingsMenuOpenFeedback.PlayFeedbacks();
    }

    private void CloseSettings()
    {
        settingsMenuCloseFeedback.PlayFeedbacks();
        mainMenuOpenAgainFeedback.PlayFeedbacks();
    }

    private void OpenCredits()
    {
        mainMenuCloseFeedback.PlayFeedbacks();
        creditsMenuOpenFeedback.PlayFeedbacks();
    }

    private void CloseCredits()
    {
        creditsMenuCloseFeedback.PlayFeedbacks();
        mainMenuOpenAgainFeedback.PlayFeedbacks();
    }

    private void OpenExitCheck()
    {
        mainMenuCloseFeedback.PlayFeedbacks();
        exitMenuOpenFeedback.PlayFeedbacks();
    }

    private void CloseExitCheck()
    {
        exitMenuCloseFeedback.PlayFeedbacks();
        mainMenuOpenAgainFeedback.PlayFeedbacks();
    }

    private void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
        EditorApplication.ExitPlaymode();
    }
}
