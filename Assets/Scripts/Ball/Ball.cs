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
}
