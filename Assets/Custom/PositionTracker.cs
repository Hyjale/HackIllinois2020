using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [Header("Most parent player object")]
    [SerializeField] GameObject player;

    [Header("")]
    [SerializeField] string filePath;

    private string playerID;

    private void Update()
    {
        addRecord(playerID, player.transform.position.x, player.transform.position.z, filePath);
        delayRecord();
    }

    private void addRecord(string ID, float x, float z, string filePath)
    {
        print("Writing to file");
        try
        {
            using (StreamWriter file = new StreamWriter(@filePath, true))
            {
                file.WriteLine(ID + "," + x + "," + z);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong! Error: " + ex.Message);
        }
    }

    private IEnumerator delayRecord()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
