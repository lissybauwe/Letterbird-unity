using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField ageInputField;
    public TMP_InputField weightInputField;
    public TMP_InputField heightInputField;
    public TMP_InputField demoTimeInputField;
    public bool toggle = true;
    public bool toggleHR = true;
    public ConnectErgometer connectErgometer;
    public GameObject connectedImage;
    public GameObject notConnected;
    public GameObject ergometer;

    private float timer = 0f;
    private float delay = 4f; // set the delay in seconds
    private bool startTimer;

    private void Awake()
    {
        // Replace "YourTag" with the actual tag you want to search for
        GameObject objWithTag = GameObject.FindWithTag("ergometer");

        if (objWithTag == null)
        {
            if (ergometer != null)
            {
                Vector2 position = new Vector2(0, 0);
                Instantiate(ergometer, position, Quaternion.identity);
            }
        }

        GameObject GO_ergometer = GameObject.FindWithTag("ergometer");
        connectErgometer = GO_ergometer.GetComponent<ConnectErgometer>();
    }

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

    public void saveSexMale()
    {
        PlayerPrefs.SetInt("Sex", 1);
        Debug.Log("Saved Male");
    }

    public void saveSexFemale()
    {
        PlayerPrefs.SetInt("Sex", 2);
        Debug.Log("Saved Female");
    }

    public void PlayGame()
    {

        PlayerPrefs.SetInt("Demo",0);
        SceneManager.LoadScene("WorldMap");

    }

    public void PlayDemo()
    {
        PlayerPrefs.SetInt("Demo",1);
        SceneManager.LoadScene("Startup_Blackmire");
    }

    public void Quit()
    {
        Application.Quit();
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

    public void saveDemoTime()
    {
        int enteredNumber;
        if(int.TryParse(demoTimeInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetInt("DemoTime", enteredNumber);
            PlayerPrefs.Save();
            Debug.Log("DemoTime saved: " + enteredNumber);
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

    public void saveHR()
    {
        toggleHR = !toggleHR;
        PlayerPrefs.SetInt("useHR", toggleHR ? 1 : 0);
        Debug.Log("useHR saved: " + toggleHR);
        PlayerPrefs.Save();
    }

    public void connectErgometerVisual()
    {
        startTimer = true;
        connectedImage.SetActive(false);
        notConnected.SetActive(false);
    }

    public void resetCollectibles()
    {
        PlayerPrefs.SetInt("CollectedOne",0);
        PlayerPrefs.SetInt("CollectedTwo", 0);
        PlayerPrefs.SetInt("CollectedThree", 0);
    }
}
