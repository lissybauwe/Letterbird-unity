using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop_Manager : MonoBehaviour
{
    public TextMeshProUGUI coins_display;
    public GameObject cantBuy;
    public GameObject buySuccessful;
    public GameObject easterEgg;
    private int coins;
    private int noCounter;

    private void Awake()
    {
        refreshCoinDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cheatCoins();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            resetAll();
        }
    }

    private void resetAll()
    {
        PlayerPrefs.SetInt("coins",0);
        PlayerPrefs.SetInt("item_azur", 0);
        PlayerPrefs.SetInt("item_satin", 0);
        PlayerPrefs.SetInt("item_emerald", 0);
        refreshCoinDisplay();
    }

    private void refreshCoinDisplay()
    {
        coins = PlayerPrefs.GetInt("coins");
        coins_display.text = coins + " G";
    }

    public void Quit()
    {
        SceneManager.LoadScene("WorldMap");
    }

    private void checkBuy(int price, string item)
    {
        if (coins >= price)
        {
            PlayerPrefs.SetInt("coins", coins - price);
            int itemCount = PlayerPrefs.GetInt(item);
            PlayerPrefs.SetInt(item, itemCount + 1);
            refreshCoinDisplay();
            buySuccessful.SetActive(true);
            cantBuy.SetActive(false);
            easterEgg.SetActive(false);
            noCounter = 0;
        }
        else
        {
            noCounter++;
            if(noCounter < 10)
            {
                buySuccessful.SetActive(false);
                cantBuy.SetActive(true);
                easterEgg.SetActive(false);

            }
            else
            {
                buySuccessful.SetActive(false);
                cantBuy.SetActive(false);
                easterEgg.SetActive(true);
            }
        }
    }

    public void buyAzur()
    {
        checkBuy(5,"item_azur");
    }

    public void buySatin()
    {
        checkBuy(5, "item_satin");
    }

    public void buyEmerald()
    {
        checkBuy(7, "item_emerald");
    }

    private void cheatCoins()
    {
        PlayerPrefs.SetInt("coins", coins + 5);
        refreshCoinDisplay();
    }
}
