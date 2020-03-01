using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using VRTK;

public class Gaze : MonoBehaviour
{
    [SerializeField] VRTK_Pointer gazePointer;
    [SerializeField] VRTK_BasePointerRenderer gazeRenderer;

    private string gazeObject;
    private string participantID;
    private float startInteraction;
    private float endInteraction;
    private bool intro;
    private bool startWriting;

    void Start()
    {
        gazeObject = "";
        startInteraction = 0;
        endInteraction = 0;
        participantID = "1235";
        startWriting = true;

        if (gazePointer != null)
        {
            RaycastHit gazeHit = gazeRenderer.GetDestinationHit();
            gazePointer.PointerEnter(gazeHit);
            gazePointer.DestinationMarkerEnter += enterGaze;
            gazePointer.PointerExit(gazeHit);
            gazePointer.DestinationMarkerExit += exitGaze;
        }
        intro = false;
    }

    private void enterGaze(object sender, DestinationMarkerEventArgs e)
    {
        startInteraction = Time.time;
        gazeObject = e.target.name;
        Debug.Log(gazeObject);
    }

    private void exitGaze(object sender, DestinationMarkerEventArgs e)
    {
        endInteraction = Time.time;
        string filePath = GetFilePath();
        addRecord(participantID, gazeObject, startInteraction, endInteraction, filePath);
        startInteraction = 0f;
        endInteraction = 0f;
    }

    private void addRecord(string ID, string interactedObject, float startTime, float endTime, string filePath)
    {
        try
        {
            if (startWriting)
            {
                using (StreamWriter file = new StreamWriter(@filePath, false))
                {
                    if (intro == false)
                    {
                        file.WriteLine("UserID,ObjectName,InterInit,InterEnd");
                        intro = true;
                    }
                    file.WriteLine(ID + "," + interactedObject + "," + startTime + "," + endTime);
                }
                startWriting = false;
            } else
            {
                using (StreamWriter file = new StreamWriter(@filePath, true))
                {
                    if (intro == false)
                    {
                        file.WriteLine("UserID,ObjectName,InterInit,InterEnd");
                        intro = true;
                    }
                    file.WriteLine(ID + "," + interactedObject + "," + startTime + "," + endTime);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Something went wrong!");
        }
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "Gaze.csv";
    }
}
