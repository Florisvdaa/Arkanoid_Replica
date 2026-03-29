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
    private int currentLives = 3;
    private int currentScore = 0;
    private int currentLevel = 0;

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

        if (currentLives <= 0)
        {
            GameOver();
            return;
        }

        ResetGame();
    }

    private void ResetGame()
    {
        if (playerPaddle == null)
        {
            GameObject playerPaddleGO = Instantiate(playerPaddlePrefab, playerStartPos.position, Quaternion.identity);

            playerPaddle = playerPaddleGO.GetComponent<PlayerMovement>();

            playerPaddle.SetupBall(ballPrefab);

            PowerUpManager.Instance.SetPlayerPaddle(playerPaddle);
        }
        else
        {
            playerPaddle.transform.position = playerStartPos.position;

            playerPaddle.SetupBall(ballPrefab);

            // lerp back to start position
        }
    }

    public void SpawnSplitBall(Vector3 position, Vector2 direction)
    {
        GameObject newBallObj = Instantiate(ballPrefab, position, Quaternion.identity);

        Ball newBall = newBallObj.GetComponent<Ball>();
        newBall.Launch(direction.normalized);
    }

    public void LevelComplete()
    {
        currentLevel++;
        Debug.Log("Level complets");

        // Load next level (or loop, or end game)
        //if (currentLevel >= levelDefinitions.Count)
        //{
        //    GameWon();
        //}
        //else
        //{
        //    LoadLevel(currentLevel);
        //}
    }
    //public void LoadLevel(int index)
    //{
    //    BrickManager.Instance.ClearLevel();
    //    //BrickManager.Instance.LoadLevel(levelDefinitions[index]);

    //    playerPaddle.SetupBall(ballPrefab);
    //}

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    public int GetCurrentLives() => currentLives;
    public int GetCurrentScore() => currentScore;
}
