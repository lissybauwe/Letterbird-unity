using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
    public GameObject letterOne;
    public GameObject letterTwo;
    public GameObject letterThree;

    public GameObject enemySmall;
    public GameObject enemyBig;

    public ErgometerScript heartrateScript;
    public Bike_Resistance playerInfoScript;

    private int local_heartrate;
    private int hr_wanted_lower;
    private int hr_wanted_higher;
    public string[] objectTags = {"big_enemy", "small_enemy", "normal_letter", "fancy_letter"};
    private uint state = 1; //initial seed for pseudo-random number generation

    private void Start()
    {
        int age = playerInfoScript.playerAge;
        int maxHR = (int)(208 - 0.7 * age);

        hr_wanted_lower = (int)(0.7 * maxHR);
        hr_wanted_higher = (int)(0.8 * maxHR);

        StartCoroutine(LetterSpawning());
        StartCoroutine(EnemySpawning());
        StartCoroutine(ObjectDespawning());

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
            yield return new WaitForSeconds(6f); // Adjust the interval as needed

            Vector2 position = transform.position;

            position.x += 10;

            if (local_heartrate != heartrateScript.hr)
            {
                local_heartrate = heartrateScript.hr;
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
            yield return new WaitForSeconds(8f); // Adjust the interval as needed

            Vector2 position = transform.position;

            position.x += 10;

            if (local_heartrate != heartrateScript.hr)
            {
                local_heartrate = heartrateScript.hr;
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
}
