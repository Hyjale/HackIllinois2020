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

    private List<string[]> recordEntry;

    public bool saveData;
    public bool record;
    public bool recording;

    private void Start()
    {
        rightController = null;
        leftController = null;
        leftHandPosition = Vector3.zero;
        rightHandPosition = Vector3.zero;
        recordEntry = new List<string[]>();

        saveData = false;
        recording = false;
    }

    void Update()
    {
        if (!record)
        {
            if (recording)
            {
                //SaveRun();
                recording = false;
                index++;
            }

            return;
        }

        else
        {
            if (!recording)
            {
                //MakeRun();
                recording = true;
            }

            UpdateData();
            UpdateRun();
        }
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
        string[] leftData = new string[6];
        leftData[0] = index.ToString();
        leftData[1] = "Left";
        leftData[2] = leftHandPosition.x.ToString();
        leftData[3] = leftHandPosition.y.ToString();
        leftData[4] = leftHandPosition.z.ToString();
        leftData[5] = timeStamp.ToString();

        string[] rightData = new string[6];
        rightData[0] = index.ToString();
        rightData[1] = "Right";
        rightData[2] = rightHandPosition.x.ToString();
        rightData[3] = rightHandPosition.y.ToString();
        rightData[4] = rightHandPosition.z.ToString();
        rightData[5] = timeStamp.ToString();

        recordEntry.Add(leftData);
        recordEntry.Add(rightData);
    }

    public void SaveData()
    {
        string[][] output = new string[recordEntry.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = recordEntry[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int j = 0; j < length; j++)
        {
            sb.AppendLine(string.Join(delimiter, output[j]));
        }

        string filePath = GetFilePath();

        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "HandData.csv";
    }
}
