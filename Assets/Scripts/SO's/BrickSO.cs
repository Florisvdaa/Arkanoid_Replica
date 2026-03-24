using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BrickSO", menuName = "ScriptableObjects/Brick")]
public class BrickSO : ScriptableObject
{
    public GameObject brickPrefab;
    public int hitPoints = 10;
    public int health = 1;
    public Sprite[] brickSprite;
}
