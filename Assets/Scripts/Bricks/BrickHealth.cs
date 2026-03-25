using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object for easy brick creation 
// multiple sprites etc & effects later on
public class BrickHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer brickSpriteRenderer;

    private int health;
    private int hitPoints;

    private BrickSO brick_SO;
    public void SetupBrick(BrickSO brickSO)
    {
        brick_SO = brickSO;

        if (brick_SO != null)
        {
            health = brick_SO.health;
            hitPoints = brick_SO.hitPoints;

            // Because arrays start on "0" health -1 gives us the first sprite in the array.
            brickSpriteRenderer.sprite = brick_SO.brickSprite[health - 1];
        }
    }

    private void ChangeBrickSprite()
    {
        brickSpriteRenderer.sprite = brick_SO.brickSprite[health - 1];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            SoundManager.Instance.PlaySFX("Hit");

            health--;
            if (health <= 0)
            {
                GameManager.Instance.AddScore(hitPoints);
                
                Destroy(gameObject);
            }
            else
                ChangeBrickSprite();
        }
    }
}
