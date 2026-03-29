using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }

    private List<Ball> activeBalls = new List<Ball>();

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

    public void RegisterBall(Ball ball)
    {
        if (!activeBalls.Contains(ball)) { activeBalls.Add(ball); }
    }

    public void UnregisterBall(Ball ball)
    {
        if(activeBalls.Contains(ball))
            activeBalls.Remove(ball);

        // if no ball left => lose life
        if (activeBalls.Count == 0)
            GameManager.Instance.LoseLife(ball.transform);
    }

    public int BallCount => activeBalls.Count;

    public Ball GetMainBall()
    {
        // optional: return the first ball or track a "Primary" ball
        return activeBalls.Count > 0 ? activeBalls[0] : null;
    }

    public void SplitAllBalls()
    {
        var ballsSnapshot = new List<Ball>(activeBalls);

        foreach (var ball in ballsSnapshot)
        {
            ball.SplitBall();
        }
    }
}
