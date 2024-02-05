using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoResponsiveUI : MonoBehaviour
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
    public int hr;
    private int maxHR;
    private float collectibleTime;
    private bool spawnedCollectible = true;

    public ItemGeneration ItemGenerationScript;

    private void Awake()
    {
        int demoTime = PlayerPrefs.GetInt("DemoTime");
        if (demoTime != 0) { totalTime = demoTime * 60f; }
        Debug.Log("Total-Time: " + totalTime);

        maxHR = (int)(208 - 0.7 * PlayerPrefs.GetInt("playerAge"));

        //Generate Time Frame in which the Collectible is spawned
        float randomPercentage = Random.Range(60f, 80f);
        collectibleTime = totalTime * (randomPercentage / 100f);
        Debug.Log("collectibleTime: " + collectibleTime);
        spawnedCollectible = false;

        timeBarSize = timeBarSlider.anchoredPosition;

        pidgeonCoordinates = pidgeon.transform.localPosition;

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
    }

    // Update is called once per frame
    void Update()
    {
        //changing Heartrate UI Element
        hr = heartRateScript.hr;
        if (PlayerPrefs.GetInt("useHR") != 1)
        {

            heartrateText.text = "X";
            if (currentTime < (0.25 * totalTime)) { 
                hr = maxHR;
            }
            if (currentTime >= (0.25 * totalTime) && currentTime < (0.35 * totalTime))
            {
                hr = (int) (0.75 * maxHR);
            }
            if (currentTime >= (0.35 * totalTime) && currentTime < (0.6 * totalTime))
            {
                hr = (int) (0.5 * maxHR);
            }
            if (currentTime >= (0.6 * totalTime) && currentTime < (0.85 * totalTime))
            {
                hr = (int) (0.75 * maxHR);
            }
            if (currentTime >= (0.85 * totalTime))
            {
                hr = maxHR;
            }
        }
        else
        {
            heartrateText.text = hr.ToString();
        }


        //changing the position of the timer bar and pidgeon

        // Update the current time based on real-time (Time.deltaTime)
        currentTime += Time.deltaTime;

        if(currentTime >= collectibleTime && spawnedCollectible == false)
        {
            spawnedCollectible = true;
            Debug.Log("SpawnCollectible");
            ItemGenerationScript.SpawnCollectible();
        }

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

            SceneManager.LoadScene("EndOfDemo");
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (PlayerPrefs.GetInt("Demo") == 0)
            {
                SceneManager.LoadScene("EndOfRun");
            }
            else
            {
                SceneManager.LoadScene("EndOfDemo");
            }
        }
    }

}

