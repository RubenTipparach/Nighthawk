using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class VRTK_Select_Addon : MonoBehaviour
{
    public VRTK_DestinationMarker pointer;
    public Color hoverColor = Color.cyan;
    public Color selectColor = Color.yellow;
    public bool logEnterEvent = true;
    public bool logHoverEvent = false;
    public bool logExitEvent = true;
    public bool logSetEvent = true;

    [Serializable]
    public class DestinationMarkerEvent : UnityEvent<object, DestinationMarkerEventArgs> { };

    public DestinationMarkerEvent DestinationMarkerEnterEvents;

    public DestinationMarkerEvent DestinationMarkerHoverEvents;

    public DestinationMarkerEvent DestinationMarkerExitEvents;

    public DestinationMarkerEvent DestinationMarkerSetEvents;


    protected virtual void OnEnable()
    {
        pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

        if (pointer != null)
        {
            pointer.DestinationMarkerEnter += DestinationMarkerEnter;
            pointer.DestinationMarkerHover += DestinationMarkerHover;
            pointer.DestinationMarkerExit += DestinationMarkerExit;
            pointer.DestinationMarkerSet += DestinationMarkerSet;
        }
        else
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
        }
    }

    protected virtual void OnDisable()
    {
        if (pointer != null)
        {
            pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
            pointer.DestinationMarkerHover -= DestinationMarkerHover;
            pointer.DestinationMarkerExit -= DestinationMarkerExit;
            pointer.DestinationMarkerSet -= DestinationMarkerSet;
        }
    }

    private void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
    {
        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }

        DestinationMarkerSetEvents?.Invoke(sender, e);
    }

    private void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
    {
        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }

        DestinationMarkerExitEvents?.Invoke(sender, e);
    }

    private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
    {
        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }

        DestinationMarkerHoverEvents?.Invoke(sender, e);
    }

    private void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
    {
        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }

        DestinationMarkerEnterEvents?.Invoke(sender, e);
    }

    protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
        VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
    }
}
