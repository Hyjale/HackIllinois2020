using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class KeyboardClick : MonoBehaviour
{
    /*
    [Header("VRTK Components")]
    [SerializeField] VRTK_Pointer leftPointer;
    [SerializeField] VRTK_BasePointerRenderer leftBasePointerRenderer;
    [SerializeField] VRTK_Pointer rightPointer;
    [SerializeField] VRTK_BasePointerRenderer rightBasePointerRenderer;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
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
    */
}
