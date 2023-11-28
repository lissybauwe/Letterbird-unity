using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class Bike_Resistance : MonoBehaviour
{
    public ErgometerScript heartRateScript;
    public int playerAge;
    public int playerWeight;
    public int playerHeight;
    public bool playerPal;

    private int HR1;
    private int HR2;
    private int countHR1;
    private int countHR2;
    private int avgHR1;
    private int avgHR2;

    private int bikeRes;
    private int bmi;
    private int pulseMax;
    private int pulseIntended;
    private int newRes = 0;

    private int i = 0;
    private int seconds = 0;
    private int lastCalc = 0; //last Calculation; used to only calculate avgHR once per second
    private int lastResCalc = 0; //last Calculation of Resistance; used to only calculate bikeRes once per second
    private int maxRes;
    private int minRes;
    private bool decreasedIntensity = false;
    private int alc_timer = 0; // timer to adjust bikeRes in ALC
    private int alc_exception_timer = 0; // timer to adjust bikeRes in ALC (expection LL2)
    private int L1; // Calculated Bike Resistance (Load) for Load Level 1; needed for calculating target load (CTL)
    private int L2; // Calculated Bike Resistance (Load) for Load Level 2; needed for calculating target load (CTL)


    // Start is called before the first frame update
    void Start()
    {
        // to calculate the average heartrates in Load Level 1 and Load Level 2 to use in Calculated Target Load
        // variables are declared here

        HR1 = 0; HR2 = 0; countHR1 = 0; avgHR1 = 0; avgHR1 = 0; avgHR2 = 0;
        pulseMax = Mathf.RoundToInt((float)(208 - (0.7 * playerAge)));
        pulseIntended = Mathf.RoundToInt((float)0.75 * pulseIntended);
        bmi = playerWeight / ((playerHeight / 100) * (playerHeight / 100));

        if(bmi < 25 || playerPal)
        {
            decreasedIntensity = false;
            minRes = playerWeight;
            maxRes = playerWeight * 4;
        }
        else
        {
            decreasedIntensity = true;
            minRes = playerWeight / 2;
            maxRes = playerWeight * 2;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Load Levels will be calculated by seconds
        if(i >= 250) //translates round about to a second
        {
            seconds++;
            //Debug.Log("!!! " + seconds);
            i = 0;
        }
        else
        {
            i++;
        }

        // start calc average HeartRates
        //------------------------------

        // avg HeartRate in LoadLevel 1
        if(seconds > 89 && seconds < 121 && seconds != lastCalc)
        {
            lastCalc = seconds;
            HR1 = HR1 + heartRateScript.hr;
            countHR1++;
            avgHR1 = HR1 / countHR1;
        }

        // avg HeartRate in LoadLevel 2
        if (seconds > 209 && seconds < 241 && seconds != lastCalc)
        {
            lastCalc = seconds;
            HR2 = HR2 + heartRateScript.hr;
            countHR2++;
            avgHR2 = HR2 / countHR2;
        }

        // 0:00 - 1:59 - Load Level 1 (L1)
        //-------------------------------

        if (seconds < 120 && lastResCalc != seconds)
        {
            lastResCalc = seconds;
            //determine BMI to calculate appropriate Load Level; exception made with PAL incase BMI inaccurate

            if (!decreasedIntensity) // normal Load Level 1
            {
                newRes = playerWeight;
            }
            else                      // decreased Load Level 1
            {
                newRes = playerWeight / 2;
            }

            L1 = newRes;
        }

        // 2:00 - 3:59 - Load Level 2 (L2) or Automatic Load Control (ALC)
        //-----------------------------------------------------------------

        if (seconds >=120 && seconds < 240 && lastResCalc != seconds)
        {
            lastResCalc = seconds;

            if(heartRateScript.hr > 0.8 * pulseMax) //exception --> swap to ALC
            {
                if(alc_exception_timer == 0 || alc_exception_timer <= seconds - 40)
                {
                    if(newRes > minRes)
                    {
                        newRes = bikeRes - 10;
                        alc_exception_timer = seconds;
                    }
                }
            }else if (!decreasedIntensity)
            {
                newRes = playerWeight * 2;
            }
            else
            {
                newRes = playerWeight;
            }

            L2 = newRes;
        }

        // 4:00 - 4:59 - Calculated Target Load (CTL)
        //-------------------------------------------

        if (seconds >= 240 && seconds < 300 && lastResCalc != seconds)
        {
            lastResCalc = seconds;

            int avg_diff;

            if (avgHR2 == avgHR1) //exception / nicht durch 0 teilen
            {
                avgHR1 = avgHR2 - 1;
            }

            if (avgHR1 > avgHR2)
            {
                avg_diff = avgHR1 - avgHR2;
            }
            else
            {
                avg_diff = avgHR2 - avgHR1;
            }

            newRes = Mathf.RoundToInt((float)(0.9 * ((pulseIntended - avgHR2) / avg_diff) * (L2 - L1) + L2));

            if (newRes < 0 || newRes < minRes)
            {
                newRes = minRes;
            }

        }

        // 5:00 - 10:00 - Automatic Load Control (ALC)
        //--------------------------------------------

        if (seconds >= 300 && seconds < 601 && lastResCalc != seconds)
        {
            lastResCalc = seconds;

            if(heartRateScript.hr > 0.8 * pulseMax)
            {
                if((alc_timer == 0 || alc_timer <= seconds - 60) && newRes > minRes)
                {
                    newRes = bikeRes - 10;
                    alc_timer = seconds;
                }
            }

            if(heartRateScript.hr < 0.7 * pulseMax)
            {
                if((alc_timer == 0 || alc_timer <= seconds -60) && newRes < maxRes)
                {
                    newRes = bikeRes + 10;
                    alc_timer = seconds;
                }
            }
        }

        if(newRes != bikeRes)
        {
            heartRateScript.bikeRes = newRes;
            bikeRes = newRes;
        }
    }
}
