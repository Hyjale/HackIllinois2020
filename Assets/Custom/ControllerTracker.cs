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

    private List<string[]> rows;

    // Testing
    public bool saveData;

    private void Start()
    {
        rightController = null;
        leftController = null;
        leftHandPosition = Vector3.zero;
        rightHandPosition = Vector3.zero;
        rows = new List<string[]>();
        saveData = false;
    }

    void Update()
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

        // Create new entry
        CreateEntry();

        if (saveData)
        {
            SaveData();
            saveData = false;
        }
    }

    void CreateEntry()
    {
        string[] leftData = new string[5];
        leftData[0] = "Left";
        leftData[1] = leftHandPosition.x.ToString();
        leftData[2] = leftHandPosition.y.ToString();
        leftData[3] = leftHandPosition.z.ToString();
        leftData[4] = timeStamp.ToString();

        string[] rightData = new string[5];
        rightData[0] = "Right";
        rightData[1] = leftHandPosition.x.ToString();
        rightData[2] = leftHandPosition.y.ToString();
        rightData[3] = leftHandPosition.z.ToString();
        rightData[4] = timeStamp.ToString();

        rows.Add(leftData);
        rows.Add(rightData);
    }

    void SaveData()
    {
        string[][] output = new string[rows.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rows[i];
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
        return Application.dataPath + "/" + "hand_data.csv";
    }
}
