using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private float height = 0;
    public ErgometerScript script;
    private float rpm = 0;
    public Transform bird;
    
    // Update is called once per frame
    void Update()
    {
       if(rpm != script.rpm)
        {
            rpm = script.rpm;
            changeHeight();
        }

    }

    void changeHeight()
    {
        //switch (rpm)
        //{
        //    case < 40
        //        : height = 0;
        //    case >40 && < 60
        //        : height = 1;
        //    case >60 && < 80
        //        : height = 2;
        //    case >80 && < 100
        //        : height = 3;
        //    default: break;
        //}

        //transform birdo height here
    }
}
