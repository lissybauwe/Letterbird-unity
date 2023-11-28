using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Calculation : MonoBehaviour
{
    public float points = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("big_enemy"))
        {
            // Subtract points when colliding with the big enemy
            points -= 500f;
            Destroy(collision.gameObject);

            Debug.Log("Points: "+points);
        }

        if (collision.gameObject.CompareTag("small_enemy"))
        {
            // Subtract points when colliding with the small enemy
            points -= 100f;
            Destroy(collision.gameObject);

            Debug.Log("Points: " + points);
        }

        if (collision.gameObject.CompareTag("normal_letter"))
        {
            // Add points when colliding with the normal letter
            points += 100f;
            Destroy(collision.gameObject);

            Debug.Log("Points: " + points);
        }

        if (collision.gameObject.CompareTag("fancy_letter"))
        {
            // Add points when colliding with the fancy letter
            points += 500f;
            Destroy(collision.gameObject);

            Debug.Log("Points: " + points);
        }
    }
}
