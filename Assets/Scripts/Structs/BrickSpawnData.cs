using UnityEngine;

public struct BrickSpawnData
{
    public BrickSO brickSO;
    public Vector2 position;

    public BrickSpawnData(BrickSO so, Vector2 pos)
    {
        brickSO = so;
        position = pos;
    }
}
