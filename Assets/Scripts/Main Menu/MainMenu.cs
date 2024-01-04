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
    public void PlayGame()
    {

        SceneManager.LoadScene("Letterbird_Run_Startup");

    }

    public void savePlayerAge()
    {
        // Parse the text from the input field and save it as a float
        float enteredNumber;
        if (float.TryParse(ageInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetFloat("playerAge", enteredNumber);
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
        float enteredNumber;
        if (float.TryParse(weightInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetFloat("playerWeight", enteredNumber);
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
        float enteredNumber;
        if (float.TryParse(heightInputField.text, out enteredNumber))
        {
            PlayerPrefs.SetFloat("playerHeight", enteredNumber);
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
}
