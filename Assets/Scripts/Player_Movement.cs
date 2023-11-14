using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private float height = 0f;
    public ErgometerScript ergometer; //Script where the actual communication with the Ergometer happens
    private float local_rpm = 0f;
    public Transform bird;
    public float moveSpeed = 5f;
    private bool calculatePos = false;
    private Vector3 currentPosition;
    private Vector3 initialPosition;
    private Vector3 lowerPosition;
    public float aimRPM = 60f;
    private bool smoothTransition = false;
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

        //manually making the dove more responsive
        //lowerPosition = transform.position;
        //lowerPosition.y -= 0.3f;
        //transform.position = lowerPosition;
        
        //if(currentPosition.y > -2)
        //{
        //currentPosition.y -= 0.0005f;
        //}

        // Apply ease-in-out function to smooth the transition
        // t = EaseInOut(t);
        transform.position = Vector3.Lerp(initialPosition, currentPosition, t);
        //if (smoothTransition)
        //{
        //    // Increment the timer
        //    transitionTimer += Time.deltaTime;

        //    // Calculate the interpolation factor based on the timer and duration
        //    float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        //    Debug.Log(initialPosition + "|||" + currentPosition);

        //    // Use Vector3.Lerp to smoothly interpolate between initial and target positions
        //    transform.position = Vector3.Lerp(initialPosition, currentPosition, t);

        //    // Check if the transition is complete
        //    if (t >= 1.0f)
        //    {
        //        Debug.Log("Transition finished");
        //        // Reset the timer or perform any other actions when the transition is complete
        //        transitionTimer = 0.0f;
        //    }

        //    if(transform.position == currentPosition)
        //    {
        //        smoothTransition = false;
        //    }
        //}


    }
    float EaseInOut(float t)
    {
        // Apply ease-in-out function for smoother transitions
        return t < 0.5f ? 0.5f * Mathf.Pow(2 * t, 2) : 0.5f * (2 - Mathf.Pow(2 * (1 - t), 2));
    }
    void changeHeight()
    {
        //Debug.Log("changeHeight");

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
            // transform.position = newPosition;
        }


        //else
        //{

        //    if (local_rpm > aimRPM + 35 ||Input.GetKeyDown(KeyCode.Alpha1))    //y = 4
        //    {
        //        currentPosition.y = 4;
        //        Debug.Log("1");
        //    }

        //    else if ((local_rpm <= aimRPM + 35 && local_rpm > aimRPM + 25) || Input.GetKeyDown(KeyCode.Alpha2))    //y = 3
        //    {
        //        currentPosition.y = 3;
        //    }

        //    else if ((local_rpm <= aimRPM + 25 && local_rpm > aimRPM + 15) || Input.GetKeyDown(KeyCode.Alpha3))    //y = 2
        //    {
        //        currentPosition.y = 2;
        //    }

        //    else if ((local_rpm <= aimRPM + 15 && local_rpm > aimRPM + 5) || Input.GetKeyDown(KeyCode.Alpha4))    //y = 1
        //    {
        //        currentPosition.y = 1;
        //    }

        //    else if ((local_rpm <= aimRPM + 5 && local_rpm > aimRPM - 5) || Input.GetKeyDown(KeyCode.Alpha5))    //Bird in the middle - y = 0
        //    {
        //        currentPosition.y = 0;
        //    }

        //    else if ((local_rpm <= aimRPM - 5 && local_rpm > aimRPM - 15) || Input.GetKeyDown(KeyCode.Alpha6))    //y = -1
        //    {
        //        currentPosition.y = -1;
        //    }

        //    else if ((local_rpm <= aimRPM - 15 && local_rpm > aimRPM - 25) || Input.GetKeyDown(KeyCode.Alpha7))    //y = -2
        //    {
        //        currentPosition.y = -2;
        //    }

        //    else if ((local_rpm <= aimRPM - 25) || Input.GetKeyDown(KeyCode.Alpha8))    //y = -3
        //    {
        //        currentPosition.y = -3;
        //        Debug.Log("8");
        //    }

        //}

        smoothTransition = true;
        
        //transform.position = currentPosition;
        
        calculatePos = false;
        
    }
}
