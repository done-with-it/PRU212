using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the SpriteRenderer and BoxCollider2D components
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Disable the components initially
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemColettor.CoinCollected)
        {
            // Enable SpriteRenderer and BoxCollider2D
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }
}
