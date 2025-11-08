using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : CellItem
{
    [SerializeField] private Sprite defaultSpriteImage; // Reference to the sprite renderer component

    private GridManager gridManager; // Reference to the GridManager
    private BlastManager blastManager; // Reference to the BlastManager
    private FallManager fallManager; // Reference to the FallManager
    private MoveManager moveManager; // Reference to the MoveManager
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer component
    // Start is called before the first frame update
    void Start()
    {
        canFall = false;
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
        moveManager.DecreaseObstacleCount("s");
    }

    public override void TakeDamage(bool tnt)
    {
        if(tnt)
        {
            Blast();
        }
    }

}
