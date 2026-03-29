using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    private PowerUpSO data;
    private float fallSpeed = 2f;
    [SerializeField] private SpriteRenderer icon;

    private float killTimer = 5f;

    public void Initialize(PowerUpSO powerUpData)
    {
        data = powerUpData;

        icon.sprite = powerUpData.icon;
    }

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    
        float timer = Time.deltaTime;
        
        if (timer > killTimer) 
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paddle"))
        {
            PowerUpManager.Instance.ActivatePowerUp(data);
            Destroy(gameObject);
        }
    }
}
