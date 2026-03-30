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

    [SerializeField] private List<LevelSO> levelDefinitions = new List<LevelSO>();

    private PlayerMovement playerPaddle;

    public bool IsTransitioning { get; private set; }

    // amount of lives until game over
    private int currentLives = 3;
    private int maxLives = 5;
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

        SetupGame();
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

        SetupGame();
    }

    public void AddLife()
    {
        currentLives++;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        OnLivesChanged?.Invoke(currentLives);
    }
    private void SetupGame()
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

            // Destroy falling powerups
            // needs to be cleaner
            foreach (var pu in GameObject.FindGameObjectsWithTag("PowerUp"))
            {
                Destroy(pu);
            }

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

        //Load next level(or loop, or end game)
        if (currentLevel >= levelDefinitions.Count)
        {
            GameWon();
        }
        else
        {
            //LoadLevel(currentLevel);
            StartCoroutine(LevelTransitionCO(currentLevel));
        }
    }

    private IEnumerator LevelTransitionCO(int nextLevelIndex)
    {
        IsTransitioning = true;

        // Disable Paddle movement
        playerPaddle.enabled = false;

        // Destroy falling powerups
        foreach (var pu in GameObject.FindGameObjectsWithTag("PowerUp"))
        {
            Destroy(pu);
        }

        // Destroy all balls
        foreach (var ball in FindObjectsOfType<Ball>())
        {
            BallManager.Instance.UnregisterBall(ball);
            Destroy(ball.gameObject);
        }

        playerPaddle.ResetPowreUps();

        // Lerp paddle to center
        Vector3 startPos = playerPaddle.transform.position;
        Vector3 targetPos = playerStartPos.position;

        float t = 0f;
        float speed = 2f;

        while(t < 1f)
        {
            t += Time.deltaTime * speed;
            playerPaddle.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Load next level
        BrickManager.Instance.ClearLevel();
        BrickManager.Instance.LoadLevel(levelDefinitions[nextLevelIndex]);

        // Spawn new ball
        playerPaddle.SetupBall(ballPrefab);

        // Re-enable paddle
        playerPaddle.enabled = true;
        
        IsTransitioning = false;
    }

    public void LoadLevel(int index)
    {
        BrickManager.Instance.ClearLevel();
        BrickManager.Instance.LoadLevel(levelDefinitions[index]);

        playerPaddle.SetupBall(ballPrefab);
    }

    public void GameWon()
    {
        Debug.Log("Game Won");
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    public int GetCurrentLives() => currentLives;
    public int GetCurrentScore() => currentScore;
}
