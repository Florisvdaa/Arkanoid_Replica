using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        settingScreen.SetActive(false);
        creditsScreen.SetActive(false);
        quitScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    private void StartGame()
    {
        Debug.Log("Start Game!");
        // SceneManager.LoadScene("GameScene");
    }

    private void OpenSettings()
    {
        settingScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    private void CloseSettings()
    {
        settingScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    private void OpenCredits()
    {
        creditsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    private void CloseCredits()
    {
        creditsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    private void OpenExitCheck()
    {
        quitScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    private void CloseExitCheck()
    {
        quitScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    private void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
