using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player position & movemnet settings")]
    [SerializeField] private GameObject playerPaddlePrefab;
    [SerializeField] private Transform playerStartPos;

    [SerializeField] private GameObject ballPrefab;

    private PlayerMovement playerPaddle;

    // amount of lives until game over
    [SerializeField] private int lives = 3;

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
            // lerp back to start position
        }

    }
}
