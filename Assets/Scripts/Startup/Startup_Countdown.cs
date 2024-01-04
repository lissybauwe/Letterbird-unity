using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Startup_Countdown : MonoBehaviour
{

    public ErgometerScript rpm_script;
    public int local_rpm;
    public int timer = 0;
    public TextMeshProUGUI countdown;

    private void Start()
    {
        countdown.text = "5";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        local_rpm = rpm_script.rpm;

        if (local_rpm >= 60)
        {
            timer++;
        }

        if(local_rpm < 60)
        {
            timer = 0;
            countdown.text = "5";
        }

        if(timer >= 300)
        {
            SceneManager.LoadScene("Letterbird_Run");
        }

        if(timer >= 250)
        {
            countdown.text = "0";
        }else if (timer >= 200)
        {
            countdown.text = "1";
        }
        else if (timer >= 150)
        {
            countdown.text = "2";
        }
        else if (timer >= 100)
        {
            countdown.text = "3";
        }
        else if (timer >= 50) 
        {
            countdown.text = "4";
        }
    }
}
