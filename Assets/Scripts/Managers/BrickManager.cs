using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the bricks
/// </summary>
public class BrickManager : MonoBehaviour
{
    public static BrickManager Instance {  get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int rows = 5;      
    [SerializeField] private int columns = 10;
    [SerializeField] private float spacingX = 0.2f;
    [SerializeField] private float spacingY = 0.2f;

    [Header("Brick types")]
    [SerializeField] private List<BrickSO> brickSOs = new();

    [Header("Level parent")]
    [SerializeField] private Transform brickParent;

    [SerializeField] private int activeBricks = 0;

    private IBrickLayoutProvider layoutProvider;
    private IBrickFactory brickFactory;

    private LevelPattern currentLevelPattern;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        layoutProvider = new GridLayoutProvider(rows, columns, spacingX, spacingY);
        brickFactory = new BrickFactory(brickParent);
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        CenterParentToCamera();
        
        if (brickSOs == null || brickSOs.Count == 0)
        {
            Debug.LogWarning("No bricks assigned!");
            return;
        }

        // use brick to calculate size (assuming all bricks have the same size
        float brickW = GetBrickWidth(brickSOs[0]);
        float brickH = GetBrickHeight(brickSOs[0]);

        float levelWidth = (columns * brickW) + ((columns - 1) * spacingX);
        float levelHeight = (rows * brickH) + ((rows - 1) * spacingY);

        Vector2 topLeft = new Vector2(-levelWidth / 2f + brickW / 2f, levelHeight / 2f - brickH / 2f);

        if (layoutProvider is GridLayoutProvider grid)
        {
            grid.SetTopLeft(topLeft, brickW, brickH);

            switch(currentLevelPattern)
            {
                case LevelPattern.Default:
                    grid.patternFunc = DefaultPattern;
                    break;
                case LevelPattern.CheckerboardPattern:
                    grid.patternFunc = CheckerboardPattern;
                    break;
                case LevelPattern.SkipPattern:
                    grid.patternFunc = SkipPattern;
                    break;
                case LevelPattern.PyramidPattern:
                    grid.patternFunc = PyramidPattern;
                    break;
                case LevelPattern.RandomPattern:
                    grid.patternFunc = RandomPattern;
                    break;
            }
        }


        List<BrickSpawnData> bricksToSpawn = layoutProvider.GenerateLayout(brickSOs);
        
        foreach (var brick in bricksToSpawn)
        {
            brickFactory.SpawnBrick(brick.brickSO, brick.position);
        }
    }

    public void RegisterBrick()
    {
        activeBricks++;
    }

    public void UnregisterBrick()
    {
        activeBricks--;

        if (activeBricks <= 0)
            GameManager.Instance.LevelComplete();
    }
    public void ClearLevel()
    {
        foreach (Transform child in brickParent)
            Destroy(child.gameObject);

        activeBricks = 0;
    }

    public void LoadLevel(LevelSO definition)
    {
        currentLevelPattern = definition.levelPattern;
        GenerateLevel();
    }

    private void CenterParentToCamera()
    {
        Vector3 camPos = Camera.main.transform.position;
        brickParent.position = new Vector3(camPos.x, camPos.y, brickParent.position.z);
    }

    private float GetBrickWidth(BrickSO brick)
    {
        var sr = brick.brickPrefab.GetComponentInChildren<SpriteRenderer>();
        return sr.bounds.size.x;
    }
    private float GetBrickHeight(BrickSO brick) 
    {
        var sr = brick.brickPrefab.GetComponentInChildren<SpriteRenderer>();
        return sr.bounds.size.y;
    }

    // Patterns
    private BrickSO CheckerboardPattern(int row, int col, BrickSO defaultType)
    {
        return (row + col) % 2 == 0 ? defaultType : brickSOs[1];
    }
    private BrickSO SkipPattern(int row, int col, BrickSO defaultType)
    {
        return col % 2 == 0 ? defaultType : null;
    }
    private BrickSO PyramidPattern(int row, int col, BrickSO defaultType)
    {
        int start = row;
        int end = columns - row - 1;

        if (col < start || col > end)
            return null;

        return defaultType;
    }
    private BrickSO RandomPattern(int row, int col, BrickSO defaultType)
    {
        return brickSOs[Random.Range(0, brickSOs.Count)];
    }
    private BrickSO DefaultPattern(int row, int col, BrickSO defaultType)
    {
        return brickSOs[row % brickSOs.Count];
    }


}
