using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MotionSampler : MonoBehaviour
{
    [SerializeField] ControllerTracker cTracker;
    [SerializeField] VRTK_ControllerEvents controllerEvents;
    bool record = false;

    private void OnEnable()
    {

        controllerEvents.ButtonOnePressed += HandleRecord;
        controllerEvents.ButtonTwoPressed += HandleSave;
    }

    private void OnDisable()
    {
        controllerEvents.ButtonOnePressed -= HandleRecord;
        controllerEvents.ButtonTwoPressed -= HandleSave;
    }

    void HandleRecord(object sender, ControllerInteractionEventArgs e)
    {
        print("RECORD");
        record = !record;
        if (record)
        {
            cTracker.record = true;
        }

        else
        {
            cTracker.record = false;
        }
    }

    void HandleSave(object sender, ControllerInteractionEventArgs e)
    {
        cTracker.SaveData();
    }
}
