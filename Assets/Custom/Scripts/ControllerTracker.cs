using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using VRTK;

public class ControllerTracker : MonoBehaviour
{
    // Controllers
    private GameObject rightController;
    private GameObject leftController;

    // Data values
    [SerializeField] Vector3 leftHandPosition;
    [SerializeField] Vector3 rightHandPosition;
    [SerializeField] float timeStamp;
    [SerializeField] int index;

    private bool startWriting;

    public string participantID = "someID";

    [SerializeField] int dropRate = 3;
    private int dropIndex = 0;
    string filePath;

    private void Start()
    {
        rightController = null;
        leftController = null;
        leftHandPosition = Vector3.zero;
        rightHandPosition = Vector3.zero;
        
        filePath = GetFilePath();
        startWriting = true;
    }

    void Update()
    {
        if (dropIndex == 0)
        {
            UpdateData();
            UpdateRun();
        }

        dropIndex++;
        dropIndex = dropIndex % dropRate;
    }

    void UpdateData()
    {
        if (rightController == null)
        {
            // Check if actual
            rightController = VRTK_DeviceFinder.GetControllerRightHand(true);
        }

        if (leftController == null)
        {
            // Check if actual
            leftController = VRTK_DeviceFinder.GetControllerLeftHand(true);
        }

        if (rightController != null)
        {
            rightHandPosition = rightController.transform.localPosition;
        }

        if (leftController != null)
        {
            leftHandPosition = leftController.transform.localPosition;
        }

        timeStamp = Time.time;
    }

    void UpdateRun()
    {
        string[] leftData = new string[7];
        leftData[0] = index.ToString();
        leftData[1] = participantID;
        leftData[2] = "Left";
        leftData[3] = leftHandPosition.x.ToString();
        leftData[4] = leftHandPosition.y.ToString();
        leftData[5] = leftHandPosition.z.ToString();
        leftData[6] = timeStamp.ToString();

        string[] rightData = new string[7];
        rightData[0] = index.ToString();
        rightData[1] = participantID;
        rightData[2] = "Right";
        rightData[3] = rightHandPosition.x.ToString();
        rightData[4] = rightHandPosition.y.ToString();
        rightData[5] = rightHandPosition.z.ToString();
        rightData[6] = timeStamp.ToString();

        WriteDataLine(leftData);
        WriteDataLine(rightData);
    }

    public void WriteDataLine(string[] line)
    {
        print("Writing to file");
        try
        {
            if (startWriting)
            {
                using (StreamWriter file = new StreamWriter(@filePath, false))
                {
                    file.WriteLine("Index" + "," + "ID" + "," + "Hand" + "," + "XPos" + "," + "YPos" +
                        "," + "ZPos" + "," + "Time");
                }
                startWriting = false;
            }
            else
            {
                using (StreamWriter file = new StreamWriter(@filePath, true))
                {
                    file.WriteLine(line[0] + "," + line[1] + "," + line[2] + "," + line[3] 
                        + "," + line[4] + "," + line[5] + "," + line[6]);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong! Error: " + ex.Message);
        }
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "hand_data.csv";
    }
}
