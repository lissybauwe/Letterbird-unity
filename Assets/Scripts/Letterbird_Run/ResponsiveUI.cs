using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResponsiveUI : MonoBehaviour
{

    public TextMeshProUGUI heartrateText;
    private ConnectErgometer heartRateScript;
    public RectTransform timeBarSlider;
    public GameObject pidgeon;
    private float totalTime = 600f; // 10 minutes in seconds
    private float currentTime = 0f;
    private Vector2 timeBarSize;
    private Vector2 pidgeonCoordinates;
    public TextMeshProUGUI pointsText;

    public CSVManager writeScript;
    private string weight;
    private string height;
    private string age;
    private int heartrate;

    private void Awake()
    {
        timeBarSize = timeBarSlider.anchoredPosition;

        pidgeonCoordinates = pidgeon.transform.localPosition;

        weight = PlayerPrefs.GetInt("playerWeight").ToString();
        height = PlayerPrefs.GetInt("playerHeight").ToString();
        age = PlayerPrefs.GetInt("playerAge").ToString();

        writeScript.writeData("Time","Heartrate", "Weight", "Age", "Height");

        // Find a GameObject with the specified tag
        GameObject ergometerManager = GameObject.FindWithTag("ergometer");

        // Check if the GameObject was found
        if (ergometerManager != null)
        {
            // Do something with the found GameObject
            Debug.Log("Found GameObject with tag: " + ergometerManager.name);
            heartRateScript = ergometerManager.GetComponent<ConnectErgometer>();
            if (heartRateScript != null)
            {
                Debug.Log("heartRateScript found");
            }
            else
            {
                Debug.Log("SCRIPT NOT FOUND");
            }
        }
        else
        {
            // Handle the case when the GameObject with the specified tag is not found
            Debug.LogError("No GameObject found with tag: YourTagName");
        }

        StartCoroutine(WriteCSV());
    }

    IEnumerator WriteCSV()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            writeScript.writeData(currentTime.ToString(), heartrate.ToString(), weight.ToString(), age.ToString(), height.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //changing Heartrate UI Element
        heartrate = heartRateScript.hr;
        heartrateText.text = heartrate.ToString();

        //changing the position of the timer bar and pidgeon

        // Update the current time based on real-time (Time.deltaTime)
        currentTime += Time.deltaTime;

        // Calculate the normalized value (0 to 1) for the slider
        float normalizedValue = currentTime / totalTime;

        float newMax = 1250;
        float newMin = 1735;
        float newMaxP = -300;
        float newMinP = -780;

        float remappedValue = (normalizedValue * (newMax - newMin)) + newMin;
        float remappedPidgeonValue = (normalizedValue * (newMaxP - newMinP)) + newMinP;

        // Set the slider value
        timeBarSlider.offsetMax = new Vector2(-remappedValue, timeBarSlider.offsetMax.y);

        pidgeonCoordinates.x = remappedPidgeonValue;

        pidgeon.transform.localPosition = pidgeonCoordinates;

        // Check if the time has reached the maximum
        if (currentTime >= totalTime)
        {
            int points = 0;
            points = int.Parse(pointsText.text);

            PlayerPrefs.SetInt("Points", points);

            int stage = PlayerPrefs.GetInt("Stage");

            if(stage == 0)
            {
                SceneManager.LoadScene("EndOfRun");
            }
            if (stage == 1)
            {
                SceneManager.LoadScene("EndOfGlimmerwood");
            }
            if (stage == 2)
            {
                SceneManager.LoadScene("EndOfBlackmire");
            }

        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            int points = 0;
            points = int.Parse(pointsText.text);

            PlayerPrefs.SetInt("Points", points);

            if (PlayerPrefs.GetInt("Demo") == 0)
            {
                int stage = PlayerPrefs.GetInt("Stage");

                if (stage == 0)
                {
                    SceneManager.LoadScene("EndOfRun");
                }
                if (stage == 1)
                {
                    SceneManager.LoadScene("EndOfGlimmerwood");
                }
                if (stage == 2)
                {
                    SceneManager.LoadScene("EndOfBlackmire");
                }
            }
            else
            {
                SceneManager.LoadScene("EndOfDemo");
            }
        }
    }

}
