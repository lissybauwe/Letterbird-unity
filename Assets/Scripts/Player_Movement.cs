using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public ErgometerScript ergometer; //Script where the actual communication with the Ergometer happens
    private float local_rpm = 0f;
    public Transform bird;
    public float moveSpeed = 5f;
    private bool calculatePos = false;
    private Vector3 currentPosition;
    private Vector3 initialPosition;
    private Vector3 lowerPosition;
    public float aimRPM = 60f;
    public float transitionDuration = 1.0f;
    private float transitionTimer = 0.0f;
    public float minRPM = 40f;
    public float maxRPM = 80f;
    public float minY = -3f;
    public float maxY = 4f;

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if ((local_rpm != ergometer.rpm || verticalInput != 0) && !calculatePos || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            calculatePos = true;
            local_rpm = ergometer.rpm;
            changeHeight();
        }
        //Debug.Log(initialPosition + "|||" + currentPosition);
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        transform.position = Vector3.Lerp(initialPosition, currentPosition, t);
       }
    void changeHeight()
    {

        float verticalInput = Input.GetAxis("Vertical");
        currentPosition = transform.position;
        initialPosition = transform.position;

        if (verticalInput != 0)
        {
            currentPosition.y += verticalInput * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Map rpm to the range [0, 1]
            float t = Mathf.InverseLerp(minRPM, maxRPM, local_rpm);

            // Clamp the interpolation factor to the range [0, 1]
            t = Mathf.Clamp01(t);

            // Use Lerp to smoothly interpolate between minY and maxY based on rpm
            float targetY = Mathf.Lerp(minY, maxY, t);

            // Update the object's position
            Vector3 newPosition = transform.position;
            newPosition.y = targetY;
            currentPosition = newPosition;
            transitionTimer = 0f;
        }        
        calculatePos = false;
        
    }
}
