using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DistanceFromSun : MonoBehaviour
{
    public Transform sunTransform;
    public Transform playerTransform;
    public VRTK_ObjectTooltip toolTip;
    public GameObject ui;

    private bool uiState;

    void Start()
    {
        uiState = false;
    }

    void Update()
    {
        float dist = Vector3.Distance(sunTransform.position, playerTransform.position);
        toolTip.displayText = "Distance from Sun: " + dist;
        Debug.Log(dist);

        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            uiState = !uiState;
            ui.SetActive(uiState);
        }
    }


}
