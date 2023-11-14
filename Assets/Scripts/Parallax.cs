using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    float startZ;
    public float speed;

    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;

    }

    public void Update()
    {
        startPosition.x -= speed;
        transform.position = startPosition * 0.01f;
    }

}
