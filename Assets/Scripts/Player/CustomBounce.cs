using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBounce : MonoBehaviour
{
    private BoxCollider2D bc2d;

    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            // Find the relative point. (Ball.x - Paddle.x) / Paddle.width
            float relativePosition = (collision.transform.position.x - transform.position.x) / bc2d.bounds.size.x;

            // Change velocity of the ball depending on the reletive point.
            collision.rigidbody.velocity = new Vector2(relativePosition, 1).normalized * collision.rigidbody.velocity.magnitude;
        }
    }
}
