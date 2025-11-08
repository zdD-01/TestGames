using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : CellItem
{
    [SerializeField] public string color; // Color of the cube
    [SerializeField] private Sprite defaultSpriteImage; // Reference to the sprite renderer component
    [SerializeField] private Sprite TNTcubeSpriteImage; // Array of sprites for each color
    
    private GridManager gridManager; // Reference to the GridManager
    private BlastManager blastManager; // Reference to the BlastManager
    private FallManager fallManager; // Reference to the FallManager
    private MoveManager moveManager; // Reference to the MoveManager

    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component


    private bool inTNTState;
    // Start is called before the first frame update

    void Awake()
    {
        // Get the SpriteRenderer component programmatically
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    { 
        canFall = true;
        isBlastable = true;
        isClickable = true;
        inTNTState = false; // Initially, it is not a TNT cube

        // Get the Managers
        gridManager = FindObjectOfType<GridManager>();
        blastManager = FindObjectOfType<BlastManager>();
        fallManager = FindObjectOfType<FallManager>();
        moveManager = FindObjectOfType<MoveManager>();

        // Get the SpriteRenderer component programmatically
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSpriteImage; // Set the default sprite image
    }


    void OnMouseDown()
    {
        if(isClickable) {
            //Debug.Log("Cube clicked! (" + position.x + ", " + position.y + ") - " + color);
            bool valid = blastManager.BlastCube(this);
            if(valid)
                moveManager.DecreaseMoveCount();
        }
        
    }
    
    public override void Blast()
    {
        gridManager.cellDestroyed(position.x, position.y);
        Destroy(gameObject);
    }

    public bool isTNTCube()
    {
        return inTNTState;
    }

    public void setTNT(bool value) {
        inTNTState = value;
        if (inTNTState) {
            if(spriteRenderer == null) 
                Debug.LogError("SpriteRenderer is null for TNT set");
            else
                spriteRenderer.sprite = TNTcubeSpriteImage; // Set the TNT sprite image
        }else {
            if(spriteRenderer == null) 
                Debug.LogError("SpriteRenderer is null for TNT set");
            else
                spriteRenderer.sprite = defaultSpriteImage; // Set the default sprite image
        }
    }
}
