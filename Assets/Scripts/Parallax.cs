using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    public float speed;

    public void Start()
    {
        startPosition = transform.position;

    }

    public void FixedUpdate()
    {
        startPosition.x -= speed;
        transform.position = startPosition * 0.05f;
    }

}
