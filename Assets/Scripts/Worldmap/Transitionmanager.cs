using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitionmanager : MonoBehaviour
{
    private void Awake()
    {
        //TODO: Logic to enable worlds (not all shown when you enter the game)
    }

    public void home()
    {
        SceneManager.LoadScene("home");
        Debug.Log("Home");
    }

    public void glimmerwood()
    {
        PlayerPrefs.SetInt("Stage",1);
        SceneManager.LoadScene("Startup_Glimmerwood");
        Debug.Log("Glimmerwood");
    }

    public void blackmire()
    {
        PlayerPrefs.SetInt("Stage", 2);
        SceneManager.LoadScene("Startup_Blackmire");
        Debug.Log("Blackmire");
    }

    public void roadsToAsbel()
    {
        PlayerPrefs.SetInt("Stage", 0);
        SceneManager.LoadScene("Startup");
    }

    public void bridgewater()
    {
        SceneManager.LoadScene("Shop");
        Debug.Log("Bridgewater");
    }


    public void saveAndQuit()
    {
        //TODO save
        SceneManager.LoadScene("StartMenu");
    }
}
