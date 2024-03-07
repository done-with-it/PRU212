using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class flip : MonoBehaviour
{
    private float dirX = 0f;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationUpdate();
    }
    private void UpdateAnimationUpdate()
    {
        
        if (dirX > 0f)
        {
            
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            
            sprite.flipX = true;
        }
    }
    }
