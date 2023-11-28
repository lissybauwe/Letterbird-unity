using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
    public GameObject letterOne;
    public GameObject letterTwo;
    public GameObject letterThree;

    public GameObject enemySmall;
    public GameObject enemyBig;

    public ErgometerScript heartrateScript;

    private int timer = 0;
    private int local_heartrate;


    void Update()
    {
        Vector2 position = transform.position;

        position.x += 10;

        if(local_heartrate != heartrateScript.hr)
        {
            local_heartrate = heartrateScript.hr;
        }
        
        // Example: Spawn object when a key is pressed (e.g., Space key)
        if (timer == 1000)
        {
            // LetterSpawning:
            // if hr in wanted range: middle area // y = 1 - y = -2
            // if hr lower than wanted range: high area // y = 3,5 - y = 1,5
            // if hr higher than wanted range: low area // y = -2,5 - y = -5

            // EnemySpawning:
            // if hr in wanted range: lower or higher area // y = -4/-5 or y = 3/4
            // if hr lower than wanted range: low to middle area // y = -5 - y = 0
            // if hr higher than wanted range: high to middle area // y = 4 - y = 0

            SpawnObject(letterOne,position);
            timer = 0;
        }
        else
        {
            timer++;
        }
    }

    void SpawnObject(GameObject item, Vector2 position)
    {
             
        // Instantiate the object at the specified position
        Instantiate(item, position, Quaternion.identity);
    }
}
