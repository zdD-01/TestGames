using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    
    [Header("Managers")]
    [SerializeField] private GridManager gridManager; // Reference to the GridManager
    [SerializeField] private BlastManager blastManager; // Reference to the FallManager

     [Header("Settings")]
    [SerializeField] private float fallDuration = 0.1f; // Duration of the fall animation
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {

        
    }

    public void TriggerFall()
    {
        HandleFalling();
    }

    private void HandleFalling()
    {
        int width = gridManager.gridArray.GetLength(0);

        for (int x = 0; x < width; x++)
        {
            StartCoroutine(FallColumn(x));
        }

        // Optional: Once fall is complete, check for newly created TNTs or further blasts
        //gridManager.CheckAndSetTNTForGrid();
    }

    private IEnumerator FallColumn(int x)
    {
        int height = gridManager.gridArray.GetLength(1);
        for (int y = height - 1; y >= 0; y--)
            {
                if (gridManager.gridArray[x, y] == null) // Check for an empty cell
                {
                    int startY = y; // Store the empty cell position
                    // Loop upward to find the first non-empty cell
                    for (int i = startY - 1; 0 <= i && i < height; i--)
                    {
                        if (gridManager.gridArray[x, i] != null) // Found a cell to fall
                        {   
                            CellItem cell = gridManager.gridArray[x, i];
                            if(cell.canFall) {
                                // Smoothly move the cube to the empty position
                                gridManager.gridArray[x, i] = null; // Clear the source cell
                                gridManager.gridArray[x, startY] = cell; // Move cube to the destination
                                
                                Vector3 targetPosition = gridManager.GridToWorldPosition(x, startY);
                                //Debug.Log("before fall 1 x: " + cell.position.x + " y: " + cell.position.y );
                                yield return StartCoroutine(SmoothFall(cell, targetPosition));
                                //Debug.Log("after fall 1");

                                // Reset scale and position after fall
                                cell.position = new Vector2Int(x, startY);

                                break; // Move only one cube per fall cycle
                            } else{
                                
                                // Spawn a new cube just under the anchored cell (unsure if this is the desired behavior)
                                if(gridManager.gridArray[x, (i + 1)] == null) {
                                    gridManager.SpawnNewCube(x, (i + 1));
                                    i += 2;
                                }
                                
                            }
                        }
                    }

                    // After falling, spawn new cubes at the top if needed
                    if (gridManager.gridArray[x, 0] == null)
                    {
                        gridManager.SpawnNewCube(x, 0);
                        y = height - 1; // Recursively check for further falls
                    }
                }
            }
        //yield return new WaitForSeconds(fallDelay); // Add a small delay between falls
        yield return null;
    }

    private IEnumerator SmoothFall(CellItem cell, Vector3 targetPosition)
    {
        bool fall = true;
        if(cell == null) {
            //Debug.Log("Cell is null");
            yield return null;
            fall = false;
        }
            
        Transform cubeTransform = cell.transform;
        Vector3 startPosition = Vector3.zero;
        if(cubeTransform == null) {
            //Debug.Log("cubeTransform is null 1");
            yield return null;
            fall = false;
        } else {
            startPosition = cubeTransform.localPosition;
        }
        
        float elapsedTime = 0f;

        while (elapsedTime < fallDuration)
        {
            // In mean time if cube is blasted return null
            if(cubeTransform == null) {
                //Debug.Log("cubeTransform is null 2");
                yield return null;
                fall = false;
                break;
            }else {
                cubeTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / fallDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        if(cubeTransform == null) {
            //Debug.Log("cubeTransform is null 1");
            fall = false;
        }
        if(fall)
            cubeTransform.localPosition = targetPosition; // Ensure exact final position
    }
}
