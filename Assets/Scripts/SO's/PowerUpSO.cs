using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Power up", menuName = "ScriptableObjects/Power-up")]
public class PowerUpSO : ScriptableObject
{
    public string powerUpName;
    public Sprite icon;
    public GameObject prefab; // the falling item prefab
    public float dropChance = 0.2f; // 20 % chance
}
