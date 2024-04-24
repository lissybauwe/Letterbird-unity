using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    public string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "playerData.csv";
    }
    public void writeData(string time, string heartrate, string weight, string age, string height, string sex)
    {
        // Save the modified data back to the CSV file
        AppendCSV(new string[] { time, heartrate, weight, age, height, sex });
    }

    void AppendCSV(string[] rowData)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open the file in append mode
            {
                writer.WriteLine(string.Join(";", rowData));
            }

            //Debug.Log("CSV file updated and appended to: " + filePath);
        }
        catch (IOException e)
        {
            //Debug.LogError("Error appending to CSV file: " + e.Message);
        }
    }
}
