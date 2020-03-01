using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapSizeCalculator : MonoBehaviour
{
    [Header("Floor must have some collider!")]
    [Header("which is (0,0).")]
    [Header("Player must be at the center of the rectangle,")]
    [Header("Fit a rectangle into the map!")]
    [SerializeField] Collider floor;
    // Start is called before the first frame update
    private void Start()
    {
        float minX = floor.bounds.min.x;
        float maxX = floor.bounds.max.x;
        float minZ = floor.bounds.min.z;
        float maxZ = floor.bounds.max.z;
        string filePath = GetFilePath();
        addRecord(minX, maxX, minZ, maxZ, filePath);
    }

    private void addRecord(float mapXMin, float mapXMax, float mapYMin, float mapYMax, string filePath)
    {
        print("Writing to file");
        try
        {
            using (StreamWriter file = new StreamWriter(@filePath, false))
            {
                file.WriteLine("MapXNeg" + "," + "MapXPos" + "," + "MapZNeg" + "," + "MapZPos");
                file.WriteLine(mapXMin + "," + mapXMax + "," + mapYMin + "," + mapYMax);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Something went wrong! Error: " + ex.Message);
        }
    }

    string GetFilePath()
    {
        return Application.dataPath + "/" + "MapSize.csv";
    }
}
