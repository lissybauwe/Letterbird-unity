using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemGeneration : MonoBehaviour
{
    public GameObject letterOne;
    public GameObject letterTwo;
    public GameObject letterThree;

    public GameObject enemySmall;
    public GameObject enemyBig;

    public GameObject hrHigh;
    public GameObject hrLow;

    public GameObject Icon_emerald;

    private ConnectErgometer heartrateScript;
    public DemoResponsiveUI demoUIScript;

    private int playerAge;

    private int local_heartrate;
    private int hr_wanted_lower;
    private int hr_wanted_higher;
    public string[] objectTags = {"big_enemy", "small_enemy", "normal_letter", "fancy_letter" };
    private uint state = 1; //initial seed for pseudo-random number generation
    private float letterspawningTime = 6f;
    private float enemyspawningTime = 8f;

    public GameObject Collectible1;
    public GameObject Collectible2;
    public GameObject Collectible3;


    private void Awake()
    {
        playerAge = PlayerPrefs.GetInt("playerAge");

        if (PlayerPrefs.GetInt("Demo") != 1) {
            int emerald = PlayerPrefs.GetInt("item_emerald");
            if (emerald > 0)
            {
                PlayerPrefs.SetInt("item_emerald", emerald - 1);
                letterspawningTime = 5f;
                enemyspawningTime = 7f;
                Icon_emerald.SetActive(true);
            }
            else
            {
                Icon_emerald.SetActive(false);
                letterspawningTime = 6f;
                enemyspawningTime = 8f;
            }
        }

        // Find a GameObject with the specified tag
        GameObject ergometerManager = GameObject.FindWithTag("ergometer");

        // Check if the GameObject was found
        if (ergometerManager != null)
        {
            // Do something with the found GameObject
            Debug.Log("Found GameObject with tag: " + ergometerManager.name);
            heartrateScript = ergometerManager.GetComponent<ConnectErgometer>();
            if (heartrateScript != null)
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

    private void Start()
    {
        int maxHR = (int)(208 - 0.7 * playerAge);

        hr_wanted_lower = (int)(0.7 * maxHR);
        hr_wanted_higher = (int)(0.8 * maxHR);

        StartCoroutine(LetterSpawning());
        StartCoroutine(EnemySpawning());
        StartCoroutine(ObjectDespawning());
        StartCoroutine(IndicateHR());

    }

    IEnumerator IndicateHR()
    {
        yield return new WaitForSeconds(1f);

        if(PlayerPrefs.GetInt("useHR")== 1)
        {
            if (local_heartrate != heartrateScript.hr)
            {
                local_heartrate = heartrateScript.hr;
            }
        }
        else
        {
            local_heartrate = demoUIScript.hr;
        }


        // if hr in wanted range:
        if (local_heartrate <= hr_wanted_higher && local_heartrate >= hr_wanted_lower)
        {
            hrHigh.SetActive(false);
            hrLow.SetActive(false);
        }

        // if hr lower than wanted range:
        if (local_heartrate < hr_wanted_lower)
        {
            hrHigh.SetActive(false);
            hrLow.SetActive(true);
        }

        // if hr higher than wanted range:
        if (local_heartrate > hr_wanted_higher)
        {
            hrHigh.SetActive(true);
            hrLow.SetActive(false);
        }
    }

    IEnumerator ObjectDespawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);


            foreach (string tag in objectTags)
            {
                // Finde alle GameObjects mit dem aktuellen Tag
                GameObject[] objectsToDespawn = GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject obj in objectsToDespawn)
                {
                    // Überprüfe, ob das Objekt links vom Spieler ist und die x-Koordinate kleiner als despawnXPosition ist
                    if (obj.transform.position.x < (transform.position.x - 10))
                    {
                        Destroy(obj);
                    }
                }
            }
        }
    }
    IEnumerator LetterSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(letterspawningTime); // Adjust the interval as needed

            Vector2 position = transform.position;

            position.x += 10;

            if(PlayerPrefs.GetInt("useHR") == 1)
                {
                    if (local_heartrate != heartrateScript.hr)
                    {
                        local_heartrate = heartrateScript.hr;
                    }
                }
            else
                {
                    local_heartrate = demoUIScript.hr;
                }

            // if hr in wanted range: middle area // y = 1 - y = -2
            if (local_heartrate <= hr_wanted_higher && local_heartrate >= hr_wanted_lower)
            {
                position.y = GenerateRandomNumber(-2f, 1f);
            }

            // if hr lower than wanted range: high area // y = 3,5 - y = 1,5
            if (local_heartrate < hr_wanted_lower)
            {
                position.y = GenerateRandomNumber(1.5f, 3.5f);
            }

            // if hr higher than wanted range: low area // y = -2,5 - y = -3
            if (local_heartrate > hr_wanted_higher)
            {
                position.y = GenerateRandomNumber(-3f, -2f);
            }

            // Choose which Letter to spawn
            GameObject letterOption = RandomlyChooseBetweenObjects(letterTwo, letterThree);
            GameObject letter = RandomlyChooseBetweenObjects(letterOption, letterOne);

            SpawnObject(letter, position);
        }
    }

    IEnumerator EnemySpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyspawningTime); // Adjust the interval as needed

            Vector2 position = transform.position;

            position.x += 10;

            if (PlayerPrefs.GetInt("useHR") == 1)
            {
                if (local_heartrate != heartrateScript.hr)
                {
                    local_heartrate = heartrateScript.hr;
                }
            }
            else
            {
                local_heartrate = demoUIScript.hr;
            }

            // if hr in wanted range: lower or higher area // y = -4/-5 or y = 3/4
            if (local_heartrate <= hr_wanted_higher && local_heartrate >= hr_wanted_lower)
            {
                float optionA = GenerateRandomNumber(-3f, -4f);
                float optionB = GenerateRandomNumber(3f, 4f);

                position.y = GenerateRandomNumber(0, 2) < 1 ? optionA : optionB;
                Debug.Log("Mid Range");
            }

            // if hr lower than wanted range: low to middle area // y = -5 - y = 0
            if (local_heartrate < hr_wanted_lower)
            {
                position.y = GenerateRandomNumber(-3f, 0f);
                Debug.Log("Too Low");
            }

            // if hr higher than wanted range: high to middle area // y = 4 - y = 0
            if (local_heartrate > hr_wanted_higher)
            {
                position.y = GenerateRandomNumber(4f, 0f);
                Debug.Log("Too High");
            }

            // Choose which Cloud to spawn
            GameObject cloudOption = RandomlyChooseBetweenObjects(enemySmall, enemyBig);
            GameObject cloud = RandomlyChooseBetweenObjects(cloudOption, enemySmall);

            SpawnObject(cloud, position);
        }
    }

    // Custom random number generator using Xorshift algorithm
    float CustomRandomFloat()
    {
        state ^= (state << 21);
        state ^= (state >> 35);
        state ^= (state << 4);
        return (float)state / uint.MaxValue;
    }

    // Method to generate a random float between a and b
    float GenerateRandomNumber(float a, float b)
    {
        return a + CustomRandomFloat() * (b - a);
    }

    GameObject RandomlyChooseBetweenObjects(GameObject a, GameObject b)
    {
        return GenerateRandomNumber(0, 2) < 1 ? a : b;
    }

    void SpawnObject(GameObject item, Vector2 position)
    {
             
        // Instantiate the object at the specified position
        Instantiate(item, position, Quaternion.identity);
    }

    public void SpawnCollectible()
    {
        Vector2 position = transform.position;
        position.x += 10;
        Debug.Log("Spawning!!!!");

        if (PlayerPrefs.GetInt("CollectedOne")!=1)
        {
            GameObject item = Collectible1;

            Instantiate(item, position, Quaternion.identity);
            Debug.Log("One");
        }
        else
        {
            if (PlayerPrefs.GetInt("CollectedTwo") != 1)
            {
                GameObject item = Collectible2;

                Instantiate(item, position, Quaternion.identity);
                Debug.Log("Two");
            }
            else
            {
                if (PlayerPrefs.GetInt("CollectedThree") != 1)
                {
                    GameObject item = Collectible3;

                    Instantiate(item, position, Quaternion.identity);
                    Debug.Log("Three");
                }
            }
        }
        
    }
}
