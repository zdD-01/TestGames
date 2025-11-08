using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject redCubePrefab;
    [SerializeField] private GameObject greenCubePrefab;
    [SerializeField] private GameObject blueCubePrefab;
    [SerializeField] private GameObject yellowCubePrefab;
    [SerializeField] private GameObject tntPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject vasePrefab;

    [Header("Parents")]
    [SerializeField] private Transform cellListParent;      // Parent for the grid cells (CellList)
    [SerializeField] private Transform backgroundParent;    // Parent for the background (BackgroundParent)

    [Header("Grid Background")]
    [SerializeField] private Sprite gridBackgroundSprite;   // 9-sliced sprite for grid background
    private GameObject gridBackground; // Reference to the grid background object
    public CellItem[,] gridArray; // 2D array to store the grid cells

    private LevelSceneManager levelSceneManager;
    private GameObject[] cubes;
    //GameObject[] obstacles = new GameObject[] {tntPrefab, boxPrefab, stonePrefab, vasePrefab};

    // Define the cell size based on your desired grid layout size
    [Header("Cell Size")]
    [SerializeField] private float cellSize = 1f;

    void Start()
    {
        levelSceneManager = LevelSceneManager.Instance;
        cubes = new GameObject[] {redCubePrefab, greenCubePrefab, blueCubePrefab, yellowCubePrefab};
        if (levelSceneManager != null)
        {
            Debug.Log("LevelSceneManager found!");
            if(levelSceneManager.IsLevelLoadedSuccessfully())
            {
                GenerateGrid();
                CheckAndSetTNTForGrid();
                //StartCoroutine(CheckAndSetTNTAfterInitialization());
            }
            else
            {
                Debug.Log("Level not loaded successfully! yet for grid generation");
            }
        }
        else {
            Debug.Log("LevelSceneManager not found!");
        }
    }

    void Update()
    {
        if(levelSceneManager != null  && levelSceneManager.IsLevelLoadedSuccessfully())
        {
            if(levelSceneManager.IsLevelCompleted()) {
                disableMove();
                //CheckAndSetTNTForGrid();
            }
                
        }
        
    }

    void GenerateGrid()
    {
        //Debug.Log("Generating grid...");
        LevelData levelData = levelSceneManager.CurrentLevelData;
        int width = levelData.grid_width;
        int height = levelData.grid_height;
        List<string> gridLayout = levelData.grid;

        gridArray = new CellItem[width, height]; // Initialize the grid array

        // Calculate scaling factor based on the grid size
        Vector2 cellScale = new Vector2(cellSize, cellSize);
        // Optional: Create background with 9-sliced sprite
        gridBackground = CreateGridBackground(width, height, cellScale);

        // Clear previous grid
        foreach (Transform child in cellListParent)
        {
            Destroy(child.gameObject);
        }

        // Create grid cells based on level data
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string cellType = gridLayout[y * width + x];
                GameObject cell = InstantiateCell(cellType, new Vector2Int(x, y), cellScale);

                if (cell != null)
                {
                    cell.transform.SetParent(cellListParent); // Parent to CellList
                    cell.transform.localPosition = GridToWorldPosition(x, y); // Set grid position
                        
                    // Add the cell to the gridArray
                    CellItem cellItem = cell.GetComponent<CellItem>();
                    cellItem.SetScale(cellScale); 
                    gridArray[x, y] = cellItem;
                }
            }
        }
    }

    public void cellDestroyed(int x, int y) {
        gridArray[x, y] = null;
    }

    public List<CellItem> GetItemsInRange(int pivotX, int pivotY, int range)
    {
        List<CellItem> itemsInRange = new List<CellItem>();
        int minX = Mathf.Max(pivotX -(range -1)/2, 0); // Calculate the minimum X value
        int minY = Mathf.Max(pivotY -(range -1)/2, 0); // Calculate the minimum Y value
        int maxX = Mathf.Min(pivotX +(range -1)/2, gridArray.GetLength(0)-1); // Calculate the maximum X value
        int maxY = Mathf.Min(pivotY +(range -1)/2, gridArray.GetLength(1)-1); // Calculate the maximum Y value

        // Loop through all the items within the range
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                CellItem item = gridArray[x, y];
                if(item != null)
                    itemsInRange.Add(item);  // Collect the item at this position
            }
        }

        return itemsInRange;  // Return the list of all collected items
    }

    public void InstantiateTNT(Vector2Int position)
    {
        GameObject tnt = Instantiate(tntPrefab);

        tnt.transform.SetParent(cellListParent);
        tnt.transform.localPosition = GridToWorldPosition(position.x, position.y);

        CellItem cellItem = tnt.GetComponent<CellItem>();
        cellItem.position = position;
        cellItem.SetScale(new Vector2(cellSize, cellSize));
        
        gridArray[position.x, position.y] = cellItem;
    }

    private GameObject InstantiateCell(string cellType, Vector2Int position, Vector2 cellScale)
    {
        //Debug.Log("Instantiating cell: " + cellType + " at position: " + position);
        GameObject prefab = null;

        // Determine the correct prefab to instantiate based on the cell type
        switch (cellType)
        {
            case "r": // Red Cube
                prefab = redCubePrefab;
                break;
            case "g": // Green Cube
                prefab = greenCubePrefab;
                break;
            case "b": // Blue Cube
                prefab = blueCubePrefab;
                break;
            case "y": // Yellow Cube
                prefab = yellowCubePrefab;
                break;
            case "t": // TNT
                prefab = tntPrefab;
                break;
            case "bo": // Box
                prefab = boxPrefab;
                break;
            case "s": // Stone
                prefab = stonePrefab;
                break;
            case "v": // Vase
                prefab = vasePrefab;
                break;
            case "rand": //Random Cube
                prefab = cubes[Random.Range(0, cubes.Length)];
                break;
        }

        if (prefab != null)
        {
            //Debug.Log("Prefab found for cell type: " + cellType);
            GameObject cell = Instantiate(prefab);
            CellItem cellItem = cell.GetComponent<CellItem>();

            // Set the scale and position of the cell
            cellItem.SetScale(cellScale);
            cellItem.position = position; // Set the grid position
            return cell;
        }

        return null; // Return null if no valid prefab is found
    }

    private GameObject CreateGridBackground(int width, int height, Vector2 cellScale)
    {
        //Debug.Log("Creating grid background...");
        // Instantiate and resize the background with the 9-sliced sprite to fit the grid dimensions
        GameObject gridBackground = new GameObject("GridBackground", typeof(SpriteRenderer));
        gridBackground.transform.SetParent(backgroundParent);

        SpriteRenderer spriteRenderer = gridBackground.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = gridBackgroundSprite; // Assuming you assign the 9-sliced sprite here
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        // Set the sorting layer and order
        spriteRenderer.sortingLayerName = "GamePlay"; // Set the sorting mode to GamePlay
        spriteRenderer.sortingOrder = 1; // Set the sorting order to be behind other objects

        // Resize the background to fit the grid
        spriteRenderer.size = new Vector2((width * cellScale.x * 2f) + 0.1f, (height * cellScale.y * 2f) + 0.1f); // Adjust the size as needed
        gridBackground.transform.localPosition = new Vector3(0, -1, 0); // Center the background
        return gridBackground;
    }

    public List<CellItem> GetAdjacentCells(CellItem cell)
    {
        List<CellItem> adjacentCells = new List<CellItem>();

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = cell.position + dir;

            if (adjacentPos.x >= 0 && adjacentPos.x < gridArray.GetLength(0) &&
                adjacentPos.y >= 0 && adjacentPos.y < gridArray.GetLength(1))
            {
                CellItem adjacentCell = gridArray[adjacentPos.x, adjacentPos.y];

                // Check if the adjacent cell is a Cube and has the same color
                if (adjacentCell != null)
                {
                    adjacentCells.Add(adjacentCell); // Add to the list of adjacent cells
                }
            }
        }

        return adjacentCells;
    }

    public List<Cube> GetAdjacentCubes(Cube cube)
    {
        List<Cube> adjacentCubes = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>(); // Create a queue to manage cubes to explore
        HashSet<Cube> visitedCubes = new HashSet<Cube>(); // Track visited cubes

        queue.Enqueue(cube); // Start with the initial cube
        visitedCubes.Add(cube); // Mark the initial cube as visited
        adjacentCubes.Add(cube); // Add the current cube to the list

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Cube currentCube = queue.Dequeue(); // Get the next cube to explore

            foreach (Vector2Int dir in directions)
            {
                Vector2Int adjacentPos = currentCube.position + dir;

                if (adjacentPos.x >= 0 && adjacentPos.x < gridArray.GetLength(0) &&
                    adjacentPos.y >= 0 && adjacentPos.y < gridArray.GetLength(1))
                {
                    CellItem adjacentCell = gridArray[adjacentPos.x, adjacentPos.y] as CellItem;

                    // Check if the adjacent cell is a Cube and has the same color
                    if (adjacentCell is Cube adjacentCube && adjacentCube.color == currentCube.color)
                    {
                        // If it hasn't been visited yet
                        if (!visitedCubes.Contains(adjacentCube))
                        {
                            visitedCubes.Add(adjacentCube); // Mark as visited
                            adjacentCubes.Add(adjacentCube); // Add to the list of adjacent cubes
                            queue.Enqueue(adjacentCube); // Add to the queue for further exploration
                        }
                    }
                }
            }
        }

        return adjacentCubes;
    }

    public void CheckAndSetTNTIfAdjacent(Cube cube)
    {
        if (cube != null)
        {
            List<Cube> adjacentCubes = GetAdjacentCubes(cube);  // Get adjacent cubes of the same color

            if (adjacentCubes.Count >= 5)
            {
                //Debug.Log("Setting TNT for cube at position: " + cube.position);
                //cube.setTNT(true);  // Set the cube as TNT
                foreach (Cube adjacentCube in adjacentCubes)
                {
                    adjacentCube.setTNT(true); // Set the adjacent cubes as TNT
                }
            }
            else
            {
                cube.setTNT(false); // Reset the TNT status
            }
        }
    }

    public void CheckAndSetTNTForGrid()
    {
        Debug.Log("Checking and setting TNT for the grid...");

        foreach (CellItem cellItem in gridArray)
        {
            if (cellItem is Cube cube)
            {
                CheckAndSetTNTIfAdjacent(cube);
            }
        }
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        LevelData levelData = levelSceneManager.CurrentLevelData;
        int width = levelData.grid_width;
        int height = levelData.grid_height;
        // Adjust initial xPos and yPos to center the grid
        float xPos = -((width - 1) * cellSize) / 2f + gridBackground.transform.localPosition.x;  // Center the grid horizontally
        float yPos = ((height - 1) * cellSize) / 2f + gridBackground.transform.localPosition.y;  // Center the grid vertically
        return new Vector3(xPos + (x * cellSize), yPos - (y * cellSize), 0);
    }
    // Method to spawn a new cube at the top of the grid
    public void SpawnNewCube(int x, int y)
    {
        GameObject cubePrefab = cubes[Random.Range(0, cubes.Length)];
        GameObject newCube = Instantiate(cubePrefab);

        newCube.transform.SetParent(cellListParent);
        newCube.transform.localPosition = GridToWorldPosition(x, y);

        CellItem cellComponent = newCube.GetComponent<CellItem>();
        cellComponent.position = new Vector2Int(x, y);
        cellComponent.SetScale(new Vector2(cellSize, cellSize));
        gridArray[x, y] = cellComponent;
    }

    public void disableMove() {
        foreach(CellItem cell in gridArray) {
            if(cell != null && (cell is Cube || cell is TNT)) {
                cell.isClickable = false;
            }
        }
    }
}