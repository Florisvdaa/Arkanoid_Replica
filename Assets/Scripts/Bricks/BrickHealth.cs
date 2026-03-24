using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object for easy brick creation 
// multiple sprites etc & effects later on
public class BrickHealth : MonoBehaviour
{
    [SerializeField] private BrickSO brickSO;
    [SerializeField] private SpriteRenderer brickSpriteRenderer;

    private int health;

    private void Awake()
    {
        SetupBrick();
    }

    private void SetupBrick()
    {
        if (brickSO != null)
        {
            health = brickSO.health;

            // Because arrays start on "0" health -1 gives us the first sprite in the array.
            brickSpriteRenderer.sprite = brickSO.brickSprite[health - 1];
        }
    }

    private void ChangeBrickSprite()
    {
        brickSpriteRenderer.sprite = brickSO.brickSprite[health - 1];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            health--;
            if (health <= 0)
                Destroy(gameObject);
            else
                ChangeBrickSprite();
        }
    }
}
