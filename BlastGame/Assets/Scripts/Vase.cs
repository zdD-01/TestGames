using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : CellItem
{

    private int health = 2; // Health of the vase
    [SerializeField] private Sprite defaultSpriteImage; // Reference to the sprite renderer component
    [SerializeField] private Sprite ShateredSpriteImage; // Reference to the sprite renderer component
    
    private GridManager gridManager; // Reference to the GridManager
    private BlastManager blastManager; // Reference to the BlastManager
    private FallManager fallManager; // Reference to the FallManager
    private MoveManager moveManager; // Reference to the MoveManager

    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    // Start is called before the first frame update
    void Start()
    {
        canFall = true;
        isBlastable = false;
        isClickable = false;
        // Get the SpriteRenderer component programmatically
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSpriteImage; // Set the default sprite image

        // Get the Managers
        gridManager = FindObjectOfType<GridManager>();
        blastManager = FindObjectOfType<BlastManager>();
        fallManager = FindObjectOfType<FallManager>();
        moveManager = FindObjectOfType<MoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Blast()
    {
        gridManager.cellDestroyed(position.x, position.y);
        Destroy(gameObject);
        moveManager.DecreaseObstacleCount("v");
    }

    public override void TakeDamage(bool tnt)
    {
        health--;
        if(health == 1) {
            spriteRenderer.sprite = ShateredSpriteImage;

        }else if(health == 0) {
            Blast();
        }
    }
}
