using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object for easy brick creation 
// multiple sprites etc & effects later on
public class BrickHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer brickSpriteRenderer;

    [SerializeField] private List<PowerUpSO> possibleDrops;

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

            BrickManager.Instance.RegisterBrick();
        }
    }

    private void ChangeBrickSprite()
    {
        brickSpriteRenderer.sprite = brick_SO.brickSprite[health - 1];
    }

    private void TryDropPowerUp()
    {
        foreach (var drop in possibleDrops)
        {
            if (Random.value <= drop.dropChance)
            {
                GameObject obj = Instantiate(drop.prefab, transform.position, Quaternion.identity);
                obj.GetComponent<PowerUpItem>().Initialize(drop);
                return; // Only drop 1 item.
            }
        }
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
                
                // Chance to drop item
                TryDropPowerUp();

                BrickManager.Instance.UnregisterBrick();

                Destroy(gameObject);
            }
            else
                ChangeBrickSprite();
        }
    }
}
