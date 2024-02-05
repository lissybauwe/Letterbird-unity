using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class homeManager : MonoBehaviour
{
    public TextMeshProUGUI satin;
    public TextMeshProUGUI azur;
    public TextMeshProUGUI emerald;
    public GameObject potionOverview;

    private void Awake()
    {
        satin.text = PlayerPrefs.GetInt("item_satin").ToString();
        azur.text = PlayerPrefs.GetInt("item_azur").ToString();
        emerald.text = PlayerPrefs.GetInt("item_emerald").ToString();
    }

    public void showPotionOverview()
    {
        potionOverview.SetActive(true);
    }

    public void closePotionOverview()
    {
        potionOverview.SetActive(false);
    }

    public void backToWorldMap()
    {
        SceneManager.LoadScene("WorldMap");
    }
}
