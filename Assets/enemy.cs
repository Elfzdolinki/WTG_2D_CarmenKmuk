using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the kot
        if (collision.collider.GetComponent<Kot>() != null)
        {
            Destroy(gameObject); // Destroy the enemy
            return;
        }

        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            return;
        }

        if( collision.contacts[0].normal.y < - 0.5)
        {
            Destroy(gameObject);
        }
    
    }

}

