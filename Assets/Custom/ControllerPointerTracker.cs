using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRTK;

public class ControllerPointerTracker : MonoBehaviour
{
    [SerializeField] VRTK_Pointer leftPointer;
    [SerializeField] VRTK_BasePointerRenderer leftBasePointerRenderer;
    [SerializeField] VRTK_Pointer rightPointer;
    [SerializeField] VRTK_BasePointerRenderer rightBasePointerRenderer;

    private float startInteractionLeft;
    private float startInteractionRight;
    private float endInteractionLeft;
    private float endInteractionRight;
    private string participantID;

    // Start is called before the first frame update
    void Start()
    {
        startInteractionLeft = 0f;
        startInteractionRight = 0f;
        endInteractionLeft = 0f;
        endInteractionRight = 0f;
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
            leftPointer.DestinationMarkerEnter += exitLeft;

            rightPointer.PointerExit(rightControllerHit);
            rightPointer.DestinationMarkerEnter += exitRight;
        }
    }

    private void enterLeft(object sender, DestinationMarkerEventArgs e)
    {
        startInteractionLeft = Time.time;
    }

    private void enterRight(object sender, DestinationMarkerEventArgs e)
    {
        startInteractionRight = Time.time;
    }

    private void exitLeft(object sender, DestinationMarkerEventArgs e)
    {
        endInteractionLeft = Time.time;
        addRecord(participantID, "Left", startInteractionLeft, endInteractionLeft, "PointerPosition.csv");

        startInteractionLeft = 0f;
        endInteractionLeft = 0f;
    }

    private void exitRight(object sender, DestinationMarkerEventArgs e)
    {
        endInteractionRight = Time.time;
        addRecord(participantID, "Right", startInteractionRight, endInteractionRight, "PointerPosition.csv");

        startInteractionRight = 0f;
        endInteractionRight = 0f;
    }

    private void addRecord(string ID, string controllerType, float startTime, float endTime, string filePath)
    {
        try
        {
            using (StreamWriter file = new StreamWriter(@filePath, true))
            {
                file.WriteLine(ID + "," + controllerType + "," + startTime + "," + endTime);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong!");
        }
    }
}
