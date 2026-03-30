using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    private PlayerMovement playerPaddle;

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

    public void SetPlayerPaddle(PlayerMovement paddle)
    {
        playerPaddle = paddle;
    }

    public void ActivatePowerUp(PowerUpSO powerUp)
    {
        switch (powerUp.powerUpName)
        {
            case "ExtendPaddle":
                playerPaddle.Extend();
                break;

            case "Magnet":
                playerPaddle.EnableMagnet();
                break;

            case "SplitBall":
                BallManager.Instance.SplitAllBalls();
                break;

            case "Life":
                GameManager.Instance.AddLife();
                break;

            default:
                Debug.LogWarning("Unknown power-up: " + powerUp.powerUpName);
                break;
        }
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.U))
        {
            BallManager.Instance.SplitAllBalls();
        }
    }
}
