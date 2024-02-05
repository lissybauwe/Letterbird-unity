using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Point_Calculation : MonoBehaviour
{
    public float points = 0f;
    public TextMeshProUGUI pointsText;
    public GameObject Satin_Icon;
    public GameObject Azur_Icon;
    private bool satin = false;
    private bool azur = false;
    private int shield = 0;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Demo") != 1)
        {
            int CountAzur = PlayerPrefs.GetInt("item_azur");
            if (CountAzur > 0)
            {
                PlayerPrefs.SetInt("item_azur", CountAzur - 1);
                azur = true;
                Azur_Icon.SetActive(true);
                shield = 5;
            }
            else
            {
                Azur_Icon.SetActive(false);
            }

            int CountSatin = PlayerPrefs.GetInt("item_satin");
            if (CountSatin > 0)
            {
                PlayerPrefs.SetInt("item_azur", CountSatin - 1);
                Satin_Icon.SetActive(true);
                satin = true;
            }
            else
            {
                Satin_Icon.SetActive(false);
            }
        }
        else
        {
            satin = false;
            azur = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("big_enemy"))
        {
            // Subtract points when colliding with the big enemy
            if(shield == 0)
            {
                points -= 500f;
            }
            else
            {
                shield = shield - 1;
                if(shield == 0)
                {
                    Azur_Icon.SetActive(false);
                }
            }
            pointsText.text = points.ToString();
            Destroy(collision.gameObject);

            //Debug.Log("Points: "+points);
        }

        if (collision.gameObject.CompareTag("small_enemy"))
        {
            // Subtract points when colliding with the small enemy
            if (shield == 0)
            {
                points -= 100f;
            }
            else
            {
                shield = shield - 1;
                if (shield == 0)
                {
                    Azur_Icon.SetActive(false);
                }
            }
            pointsText.text = points.ToString();
            Destroy(collision.gameObject);

            //Debug.Log("Points: " + points);
        }

        if (collision.gameObject.CompareTag("normal_letter"))
        {
            // Add points when colliding with the normal letter
            // if bought satinberry:
            if (satin)
            {
                points += 120f;
            }
            else
            {
                points += 100f;
            }
            pointsText.text = points.ToString();
            Destroy(collision.gameObject);

            //Debug.Log("Points: " + points);
        }

        if (collision.gameObject.CompareTag("collectible1"))
        {
            Destroy(collision.gameObject);
            PlayerPrefs.SetInt("CollectedOne", 1);
        }
        if (collision.gameObject.CompareTag("collectible2"))
        {
            Destroy(collision.gameObject);
            PlayerPrefs.SetInt("CollectedTwo", 1);
        }
        if (collision.gameObject.CompareTag("collectible3"))
        {
            Destroy(collision.gameObject);
            PlayerPrefs.SetInt("CollectedThree", 1);
        }

        if (collision.gameObject.CompareTag("fancy_letter"))
        {
            // Add points when colliding with the fancy letter
            if (satin)
            {
                points += 600f;
            }
            else
            {
                points += 500f;
            }
            pointsText.text = points.ToString();
            Destroy(collision.gameObject);

            //Debug.Log("Points: " + points);
        }
    }
}
