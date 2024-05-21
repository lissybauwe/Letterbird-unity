using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPI : MonoBehaviour
{
    public string url = "";

    public void GetDataFromAPI(string url)
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(url + "/playerInfo"))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log("received: " + json);
                FillInData(json);
            }
        }
    }

    void FillInData(string playerInfoJSON)
    {
        MainMenu menu = GetComponent<MainMenu>();
        Player player = JsonUtility.FromJson<Player>(playerInfoJSON);

        menu.ageInputField.text = player.age.ToString();
        menu.savePlayerAge();

        menu.weightInputField.text = player.weight.ToString();
        menu.savePlayerWeight();

        menu.heightInputField.text = player.height.ToString();
        menu.savePlayerHeight();

        menu.demoTimeInputField.text = player.demoTime.ToString();
        menu.saveDemoTime();

        menu.toggle = player.PAL;
        menu.savePAL();

        menu.toggleHR = player.heartrate;
        menu.saveHR();

        if (player.female)
        {
            menu.saveSexFemale();
        }
        else
        {
            menu.saveSexMale();
        }
    }

    public struct Player
    {
        public int age;
        public int weight;
        public int height;
        public int demoTime;
        public bool PAL;
        public bool heartrate;
        public bool male;
        public bool female;
    }
}
