using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField ageInputField;
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    private bool toggle = true;
    public ConnectErgometer connectErgometer;
    public GameObject connectedImage;
    public GameObject notConnected;

    private float timer = 0f;
    private float delay = 4f; // set the delay in seconds
    private bool startTimer;

    void Update()
    {
        if (startTimer)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if the delay has been reached
            if (timer >= delay)
            {
                // Reset the timer or perform your actions
                timer = 0f;

                // Code to execute after the delay
                bool connected = connectErgometer.connected;

                if (connected)
                {
                    connectedImage.SetActive(true);
                    notConnected.SetActive(false);
                }
                else
                {
                    connectedImage.SetActive(false);
                    notConnected.SetActive(true);
                }

                startTimer = false;
                timer = 0f;
            }
        }
    }


    public void PlayGame()
    {

        SceneManager.LoadScene("Letterbird_Run_Startup");

    }

    public void savePlayerAge()
    {
        // Parse the text from the input field and save it as a float
        int enteredNumber;
        if (int.TryParse(ageInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetInt("playerAge", enteredNumber);
            PlayerPrefs.Save();
            Debug.Log("Age saved: " + enteredNumber);
        }
        else
        {
            Debug.LogError("Invalid number entered!");
        }
    }

    public void savePlayerWeight()
    {
        // Parse the text from the input field and save it as a float
        int enteredNumber;
        if (int.TryParse(weightInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetInt("playerWeight", enteredNumber);
            PlayerPrefs.Save();
            Debug.Log("Weight saved: " + enteredNumber);
        }
        else
        {
            Debug.LogError("Invalid number entered!");
        }
    }

    public void savePlayerHeight()
    {
        // Parse the text from the input field and save it as a float
        int enteredNumber;
        if (int.TryParse(heightInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetInt("playerHeight", enteredNumber);
            PlayerPrefs.Save();
            Debug.Log("Height saved: " + enteredNumber);
        }
        else
        {
            Debug.LogError("Invalid number entered!");
        }
    }

    public void savePAL()
    {
        toggle = !toggle;
        PlayerPrefs.SetInt("playerPAL", toggle ? 1 : 0);
        Debug.Log("PAL saved: " + toggle);
        PlayerPrefs.Save();
    }

    public void connectErgometerVisual()
    {
        startTimer = true;
        connectedImage.SetActive(false);
        notConnected.SetActive(false);
    }
}
