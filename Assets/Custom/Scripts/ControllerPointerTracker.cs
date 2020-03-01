﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRTK;

public class ControllerPointerTracker : MonoBehaviour
{
    [Header("File Path")]
    [SerializeField] string filePath; 

    [Header("VRTK Components")]
    [SerializeField] VRTK_Pointer leftPointer;
    [SerializeField] VRTK_BasePointerRenderer leftBasePointerRenderer;
    [SerializeField] VRTK_Pointer rightPointer;
    [SerializeField] VRTK_BasePointerRenderer rightBasePointerRenderer;

    private float startInteractionLeft;
    private float startInteractionRight;
    private float endInteractionLeft;
    private float endInteractionRight;
    private string objectInteractedLeft;
    private string objectInteractedRight;
    private string participantID;

    // Start is called before the first frame update
    void Start()
    {
        startInteractionLeft = 0f;
        startInteractionRight = 0f;
        endInteractionLeft = 0f;
        endInteractionRight = 0f;
        objectInteractedLeft = "";
        objectInteractedRight = "";
        participantID = "SomeID";

        if (leftPointer != null && rightPointer != null && leftBasePointerRenderer != null && rightBasePointerRenderer != null)
        {
            RaycastHit leftControllerHit = leftBasePointerRenderer.GetDestinationHit();
            RaycastHit rightControllerHit = rightBasePointerRenderer.GetDestinationHit();

            leftPointer.PointerEnter(leftControllerHit);
            leftPointer.DestinationMarkerEnter += enterLeft;

            rightPointer.PointerEnter(rightControllerHit);
            rightPointer.DestinationMarkerEnter += enterRight;

            leftPointer.PointerExit(leftControllerHit);
            leftPointer.DestinationMarkerExit += exitLeft;

            rightPointer.PointerExit(rightControllerHit);
            rightPointer.DestinationMarkerExit += exitRight;
        }
    }

    private void enterLeft(object sender, DestinationMarkerEventArgs e)
    {
        startInteractionLeft = Time.time;
        objectInteractedLeft = e.target.name;
    }

    private void enterRight(object sender, DestinationMarkerEventArgs e)
    {
        startInteractionRight = Time.time;
        objectInteractedRight = e.target.name;
    }

    private void exitLeft(object sender, DestinationMarkerEventArgs e)
    {
        endInteractionLeft = Time.time;
        addRecord(participantID, "Left", objectInteractedLeft, startInteractionLeft, endInteractionLeft, filePath);
        print("Pointer entered " + e.target.name + " on Controller index [" + e.controllerReference + "]");
    }

    private void exitRight(object sender, DestinationMarkerEventArgs e)
    {
        endInteractionRight = Time.time;
        addRecord(participantID, "Right", objectInteractedRight, startInteractionRight, endInteractionRight, filePath);
        print("Pointer entered " + e.target.name + " on Controller index [" + e.controllerReference + "]");
    }

    private void addRecord(string ID, string controllerType, string interactedObject, float startTime, float endTime, string filePath)
    {
        print("Writing to file");
        try
        {
            using (StreamWriter file = new StreamWriter(@filePath, true))
            {
                file.WriteLine(ID + "," + controllerType + "," + interactedObject + "," + startTime + "," + endTime);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong! Error: " + ex.Message);
        }
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "PointerData.csv";
    }
}

// End of File.