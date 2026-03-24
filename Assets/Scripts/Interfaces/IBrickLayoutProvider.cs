using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stratagy pattern
/// This decides 'where' the bricks go
/// </summary>
public interface IBrickLayoutProvider 
{
    List<BrickSpawnData> GenerateLayout(List<BrickSO> brickTypes);
}

public class GridLayoutProvider : IBrickLayoutProvider
{
    private int rows;
    private int columns;
    private float spacingX;
    private float spacingY;
    private Vector2 topLeft;
    private float brickW;
    private float brickH;

    public GridLayoutProvider(int rows, int columns, float spacingX, float spacingY)
    {
        this.rows = rows;
        this.columns = columns;
        this.spacingX = spacingX;
        this.spacingY = spacingY;
    }

    public void SetTopLeft(Vector2 topLeft, float brickW, float brickH)
    {
        this.topLeft = topLeft;
        this.brickW = brickW;
        this.brickH = brickH;
    }

    public List<BrickSpawnData> GenerateLayout(List<BrickSO> brickTypes)
    {
        List<BrickSpawnData> result = new();

        for (int row = 0; row < rows; row++)
        {
            BrickSO brickType = brickTypes[row % brickTypes.Count];

            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = new Vector2(topLeft.x + col * (brickW + spacingX), topLeft.y - row * (brickH + spacingY));

                result.Add(new BrickSpawnData(brickType, pos));
            }
        }

        return result;
    }

}
