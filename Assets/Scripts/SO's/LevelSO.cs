using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO")]
public class LevelSO : ScriptableObject
{
    public int levelIndex;
    public LevelPattern levelPattern;
}

public enum LevelPattern
{
    Default,
    CheckerboardPattern,
    SkipPattern,
    PyramidPattern,
    RandomPattern,
}
