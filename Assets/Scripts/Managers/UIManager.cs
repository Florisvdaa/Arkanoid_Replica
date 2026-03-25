using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player HUD")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    [SerializeField] private TextMeshProUGUI scoreText;

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
        // UI manager subscribes to GameManager events
        GameManager.Instance.OnLivesChanged += UpdateHearts;
        GameManager.Instance.OnScoreChanged += UpdateScore;

        // Initalise Hud
        UpdateHearts(GameManager.Instance.GetCurrentLives());
        UpdateScore(GameManager.Instance.GetCurrentScore());
    }

    private void UpdateHearts(int lives)
    {
        // Clear old hearts
        foreach (Transform child in heartContainer)
            Destroy(child.gameObject);

        // Add new hearts
        for (int i = 0; i < lives; i++)
            Instantiate(heartPrefab, heartContainer);
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString("D10");
    }
}
