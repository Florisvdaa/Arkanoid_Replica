using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 15f;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        BallManager.Instance.RegisterBall(this);
    }

    public void Launch(Vector2 direciton)
    {
        transform.parent = null;
        rb2d.simulated = true;
        rb2d.velocity = direciton.normalized * speed;
    }

    public void Catch(Transform parent)
    {
        transform.parent = parent;
        rb2d.simulated = false;
        rb2d.velocity = Vector2.zero;
    }

    public void SplitBall()
    {
        Vector2 baseDir = rb2d.velocity.normalized;
        if (baseDir == Vector2.zero)
            baseDir = Vector2.up;

        Vector2 dir1 = Quaternion.Euler(0, 0, 20f) * baseDir;
        Vector2 dir2 = Quaternion.Euler(0, 0, -20f) * baseDir;

        GameManager.Instance.SpawnSplitBall(transform.position, dir1);
        GameManager.Instance.SpawnSplitBall(transform.position, dir2);
    }

    public void DestroyBall()
    {
        BallManager.Instance.UnregisterBall(this);
        Destroy(gameObject);
    }
}
