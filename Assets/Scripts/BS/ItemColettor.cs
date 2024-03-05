using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColettor : MonoBehaviour
{
    public static bool CoinCollected = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            CoinCollected = true;
        }
    }
}
