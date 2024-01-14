using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class HighscoreData
{
    public List<HighscoreEntry> highscores;
}

[Serializable]
public class HighscoreEntry
{
    public string playerName;
    public int score;
}

public class HighscoreManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreText; // Reference to a UI Text element to display highscores

    private string highscoreFilePath;

    private HighscoreData highscoreData;

    void Start()
    {
        Debug.Log("Start");
        highscoreFilePath = Application.persistentDataPath + "/highscores.dat";

        // Load existing highscores or initialize if it's the first run
        LoadHighscores();
        DisplayHighscores();
    }

    public void SaveHighscore(string playerName, int newHighscore)
    {
        // Create a new HighscoreEntry
        HighscoreEntry newEntry = new HighscoreEntry
        {
            playerName = playerName,
            score = newHighscore
        };

        // Add the new highscore entry to the list
        highscoreData.highscores.Add(newEntry);

        // Sort the list in descending order based on the score
        highscoreData.highscores.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only the top 10 highscores
        if (highscoreData.highscores.Count > 10)
        {
            highscoreData.highscores.RemoveRange(10, highscoreData.highscores.Count - 10);
        }

        // Save the updated highscores to the file
        SaveHighscores();

        // Display the updated highscores on the UI
        DisplayHighscores();
    }


    // Load existing highscores from the file
    private void LoadHighscores()
    {
        Debug.Log("LoadHighscores()");

        if (File.Exists(highscoreFilePath))
        {
            try
            {
                using (FileStream fileStream = new FileStream(highscoreFilePath, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    highscoreData = (HighscoreData)binaryFormatter.Deserialize(fileStream);
                    Debug.Log("loaded");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading highscores: " + e.Message);
                InitializeHighscores();
            }
        }
        else
        {
            Debug.Log("Initilizing...");
            InitializeHighscores();
        }
    }

    // Save highscores to the file
    private void SaveHighscores()
    {
        Debug.Log("SaveHighscores");
        try
        {
            using (FileStream fileStream = new FileStream(highscoreFilePath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, highscoreData);
                Debug.Log("Serialize");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving highscores: " + e.Message);
        }
    }

    // Initialize highscores if it's the first run
    private void InitializeHighscores()
    {
        Debug.Log("InitializeHighscores()");
        highscoreData = new HighscoreData
        {
            highscores = new List<HighscoreEntry>()
        };

        SaveHighscores();
    }

    // Display highscores on the UI
    private void DisplayHighscores()
    {
        Debug.Log("DisplayHighscores()");
        string highscoreDisplay = "Highscores:\n \n";

        for (int i = 0; i < highscoreData.highscores.Count; i++)
        {
            HighscoreEntry entry = highscoreData.highscores[i];

            if (entry != null)
            {
                highscoreDisplay += (i + 1) + ". " + entry.playerName + ": " + entry.score + "\n";
            }
            else
            {
                // Handle null entry, for example, by skipping it
                Debug.LogWarning("Null entry found at index " + i);
                InitializeHighscores();
            }
        }

        highscoreText.text = highscoreDisplay;
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void playAgain()
    {
        SceneManager.LoadScene("Startup");
    }
}

