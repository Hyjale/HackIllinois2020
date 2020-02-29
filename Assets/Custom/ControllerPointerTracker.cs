using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerPointerTracker : MonoBehaviour
{
    [SerializeField] VRTK_Pointer leftPointer;
    [SerializeField] VRTK_BasePointerRenderer leftBasePointerRenderer;
    [SerializeField] VRTK_Pointer rightPointer;
    [SerializeField] VRTK_BasePointerRenderer rightBasePointerRenderer;

    private float startInteractionTime;
    private float endInteractionTime;

    // Start is called before the first frame update
    void Start()
    {
        startInteractionTime = 0;
        endInteractionTime = 0;

        if (leftPointer != null && rightPointer != null && leftBasePointerRenderer != null && rightBasePointerRenderer != null)
        {
            RaycastHit leftControllerHit = leftBasePointerRenderer.GetDestinationHit();
            RaycastHit rightControllerHit = rightBasePointerRenderer.GetDestinationHit();

            leftPointer.PointerEnter(leftControllerHit);
            leftPointer.DestinationMarkerEnter += enter;

            rightPointer.PointerEnter(rightControllerHit);
            rightPointer.DestinationMarkerEnter += enter;

            leftPointer.PointerExit(leftControllerHit);
            leftPointer.DestinationMarkerEnter += exit;

            rightPointer.PointerExit(rightControllerHit);
            rightPointer.DestinationMarkerEnter += exit;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void enter(object sender, DestinationMarkerEventArgs e)
    {
        startInteractionTime = Time.time;
    }
    private void exit(object sender, DestinationMarkerEventArgs e)
    {
        endInteractionTime = Time.time;

    }
}
