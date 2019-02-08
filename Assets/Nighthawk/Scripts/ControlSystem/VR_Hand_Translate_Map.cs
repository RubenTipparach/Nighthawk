using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VR_Hand_Translate_Map : MonoBehaviour
{
    VRTK_ControllerEvents controllerEvents;

    Vector3 currentLocation;

    Transform controllerTransform;

    bool gripped = false;

    Vector3 offsetPosition;

    // Will move this to the game manager later.
    [SerializeField]
    Transform targetMoveObj;

    [SerializeField]
    float distanceMultiplier = 10;

    // Start is called before the first frame update
    void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        controllerEvents.GripPressed += ControllerEvents_GripPressed; ;

        controllerEvents.GripReleased += ControllerEvents_GripReleased; ;
    }

    private void ControllerEvents_GripPressed(object sender, ControllerInteractionEventArgs e)
    {
        controllerTransform = controllerEvents.transform;
        currentLocation = controllerTransform.position;
        offsetPosition = targetMoveObj.position;

        gripped = true;
    }

    private void ControllerEvents_GripReleased(object sender, ControllerInteractionEventArgs e)
    {
        gripped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gripped)
        {
            Vector3 diffPosition = (controllerTransform.position - currentLocation) * 10;

            Vector3 newPosition = new Vector3(diffPosition.x, 0, diffPosition.z) + offsetPosition;

            targetMoveObj.position = newPosition;
        }
    }
}
