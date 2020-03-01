using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [Header("Most parent player object")]
    [SerializeField] GameObject player;

    private string playerID;
    private string filePath;
    private bool intro;
    private bool startWriting;
    private bool canRecord;

    private void Start()
    {
        startWriting = true;
        intro = true;
        canRecord = true;
        playerID = "1234";
        filePath = GetFilePath();
    }

    private void Update()
    {
        if (canRecord)
        {
            addRecord(playerID, Time.time, player.transform.position.x, player.transform.position.z, filePath);
            StartCoroutine(delayRecord());
        }
    }

    private void addRecord(string ID, float time, float x, float z, string filePath)
    {
        print("Writing to file");
        try
        {
            if (startWriting)
            {
                using (StreamWriter file = new StreamWriter(@filePath, false))
                {
                    file.WriteLine("UserID" + "," + "Time" + "," + "XPos" + "," + "ZPos");
                }
                startWriting = false;
            } else
            {
                using (StreamWriter file = new StreamWriter(@filePath, true))
                {
                    file.WriteLine(ID + "," + time + "," + x + "," + z);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong! Error: " + ex.Message);
        }
    }

    private IEnumerator delayRecord()
    {
        canRecord = false;
        yield return new WaitForSeconds(0.2f);
        canRecord = true;
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "PositionTracker.csv";
    }
}
