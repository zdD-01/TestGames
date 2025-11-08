using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GridManager gridManager; // Reference to the GridManager
    [SerializeField] private FallManager fallManager; // Reference to the FallManager

    void Start()
    {
        // Get the GridManager
        //gridManager = FindObjectOfType<GridManager>();
        // Get the FallManager
        //fallManager = FindObjectOfType<FallManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool BlastCube(Cube cube) {
        // Find adjacent cellItems and take care of damage
        Vector2Int position = cube.position;
        
        List<Cube> adjacentCubes = gridManager.GetAdjacentCubes(cube);
        if (adjacentCubes.Count >= 2)
        {
            foreach (Cube c in adjacentCubes)
            {
                if(c != null) {
                    c.Blast();
                }
                CellBlasted((c as CellItem));
            }
            if(cube.isTNTCube()) {
                gridManager.InstantiateTNT(position);
            }
            fallManager.TriggerFall();
            gridManager.CheckAndSetTNTForGrid();
        }else{
            return false;
        }
        
        return true;
    }
    

    public void CellBlasted(CellItem cell, bool triggerFall = false)
    {
        bool isTNT = cell is TNT;
        bool isCube = cell is Cube;
        if(isCube)
        {
            // Find adjacent cellItems and take care of damage
            List<CellItem> adjacentItems = gridManager.GetAdjacentCells(cell);
            foreach(CellItem item in adjacentItems)
            {
                item.TakeDamage(isTNT);
            }
        } else if(isTNT)
        {
            int range = 5;
            // Find adjacent cellItems and take care of damage
            List<CellItem> adjacentItems = gridManager.GetAdjacentCells(cell);
            //check if one of them is TNT
            foreach(CellItem item in adjacentItems)
            {
                if(item is TNT)
                {
                    range = 7;
                    break;
                }
            }
            // get in range cellItems and take care of damage
            List<CellItem> inRangeItems = gridManager.GetItemsInRange(cell.position.x, cell.position.y, range);
            foreach(CellItem item in inRangeItems)
            {
                if(item is Cube || item is TNT)
                    item.Blast();
                else if(item)
                    item.TakeDamage(isTNT);
            }
        }
        if(triggerFall)
        {
            fallManager.TriggerFall();
            gridManager.CheckAndSetTNTForGrid();
        }
    }
}
