using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BrickSO", menuName = "ScriptableObjects/Brick")]
public class BrickSO : ScriptableObject
{
    // Later add Armor for the brick ( some sort of extra life)

    public int health = 1;
    public Sprite[] brickSprite;
}
