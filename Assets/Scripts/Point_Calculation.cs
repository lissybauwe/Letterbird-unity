using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Calculation : MonoBehaviour
{
    public float points = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("big_enemy"))
        {
            // Subtract points when colliding with the big enemy
            // You can replace this with your scoring system logic
            points -= 100f;

            Debug.Log("Points: "+points);
        }
    }
}
