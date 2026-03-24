using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for spawning and initialization
/// </summary>
public interface IBrickFactory
{
    void SpawnBrick(BrickSO brickData, Vector2 pos);
}

public class BrickFactory : IBrickFactory
{
    private Transform parent;

    public BrickFactory(Transform parent)
    {
        this.parent = parent;
    }

    public void SpawnBrick(BrickSO brickData, Vector2 localPos)
    {
        if (brickData.brickPrefab == null)
        {
            Debug.LogError($"Brick '{brickData.name}' has no prefab assigned!");
            return;
        }

        GameObject brickObj = Object.Instantiate(brickData.brickPrefab, parent);
        brickObj.transform.localPosition = localPos;

        BrickHealth brickHealth = brickObj.GetComponent<BrickHealth>();
        if (brickHealth != null)
            brickHealth.SetupBrick(brickData);
    }
}
