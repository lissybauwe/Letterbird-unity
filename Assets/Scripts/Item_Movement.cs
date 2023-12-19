using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Movement : MonoBehaviour
{
    public float speed;
    Vector2 startPosition;

    public void Start()
    {
        startPosition = transform.position;
    }

    public void FixedUpdate()
    {
        startPosition.x -= (speed * 0.05f);
        transform.position = startPosition;
    }
}
