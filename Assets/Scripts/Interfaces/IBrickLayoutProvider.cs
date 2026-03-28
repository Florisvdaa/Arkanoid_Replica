using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.Texture2DShaderProperty;

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

    public System.Func<int, int, BrickSO, BrickSO> patternFunc; // Returns the brick type to use (or null to skip)

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
            BrickSO defaultType = brickTypes[row % brickTypes.Count];

            for (int col = 0; col < columns; col++)
            {
                BrickSO finalType = patternFunc != null
                ? patternFunc(row, col, defaultType)
                : defaultType;

                if (finalType == null)
                    continue; // skip brick

                Vector2 pos = new Vector2(
                    topLeft.x + col * (brickW + spacingX),
                    topLeft.y - row * (brickH + spacingY)
                );

                result.Add(new BrickSpawnData(finalType, pos));

                //Vector2 pos = new Vector2(topLeft.x + col * (brickW + spacingX), topLeft.y - row * (brickH + spacingY));

                //result.Add(new BrickSpawnData(brickType, pos));
            }
        }

        return result;
    }

}
