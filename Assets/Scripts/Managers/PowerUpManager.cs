using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    private PlayerMovement playerPaddle;
    private Ball currentBall;

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

    public void SetCurrentBall(Ball ball)
    {
        currentBall = ball;
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
                currentBall.SplitBall();
                break;

            default:
                Debug.LogWarning("Unknown power-up: " + powerUp.powerUpName);
                break;
        }
    }
}
