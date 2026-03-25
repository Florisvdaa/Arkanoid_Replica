using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Events
    public event Action<int> OnLivesChanged;
    public event Action<int> OnScoreChanged;

    public static GameManager Instance { get; private set; }

    [Header("Player position & movemnet settings")]
    [SerializeField] private GameObject playerPaddlePrefab;
    [SerializeField] private Transform playerStartPos;

    [SerializeField] private GameObject ballPrefab;

    private PlayerMovement playerPaddle;

    // amount of lives until game over
    [SerializeField] private int currentLives = 3;
    private int currentScore;

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

        ResetGame();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void LoseLife(Transform pos)
    {
        SoundManager.Instance.PlaySFX("Boom");
        ParticleManager.Instance.PlayEffect("DeathVFX", pos.position);

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if(currentLives > 0)
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        if (playerPaddle == null)
        {
            GameObject playerPaddleGO = Instantiate(playerPaddlePrefab, playerStartPos.position, Quaternion.identity);

            playerPaddle = playerPaddleGO.GetComponent<PlayerMovement>();

            playerPaddle.SetupBall(ballPrefab);
        }
        else
        {
            playerPaddle.transform.position = playerStartPos.position;

            playerPaddle.SetupBall(ballPrefab);

            // lerp back to start position
        }

    }

    public int GetCurrentLives() => currentLives;
    public int GetCurrentScore() => currentScore;
}
