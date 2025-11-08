using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellItem : MonoBehaviour
{
    public Vector2Int position; // Position in the grid
    public bool canFall; // Determines if the item can fall down
    public bool isBlastable; // Determines if the item can be blasted
    public bool isClickable; // Determines if the item can be clicked

    // Scaling factor to adjust the size of each cell based on the grid size
    public Vector2 cellScale = Vector2.one;

    // Base behavior for taking damage (for obstacles)
    public virtual void TakeDamage(bool tnt) { }

    // Base behavior for falling (used for cubes and TNT)
    public virtual void Fall(Vector2Int targetPosition) 
    {
        // Example implementation of moving to targetPosition
        //transform.localPosition = (Vector2)targetPosition * cellScale;
    }

    // Abstract method to handle blasting (override in child classes)
    public abstract void Blast();

    // Method to set the scale of the cell based on the grid size
    public virtual void SetScale(Vector2 scale)
    {
        cellScale = scale;
        transform.localScale = new Vector3(scale.x, scale.y, 1f); // Set uniform scale for each cell
    }
}
