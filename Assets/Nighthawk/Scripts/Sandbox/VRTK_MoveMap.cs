using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;
using Wrld;
using Wrld.Space;

public class VRTK_MoveMap : MonoBehaviour
{
    [Header("Slide Settings")]

    [Tooltip("The maximum speed the controlled object can be moved in based on the position of the axis.")]
    public float maximumSpeed = 3f;
    [Tooltip("The rate of speed deceleration when the axis is no longer being changed.")]
    public float deceleration = 0.1f;
    [Tooltip("The rate of speed deceleration when the axis is no longer being changed and the object is falling.")]
    public float fallingDeceleration = 0.01f;
    [Tooltip("The speed multiplier to be applied when the modifier button is pressed.")]
    public float speedMultiplier = 1.5f;

    public float EarthLatLongMultiplier = 0.001f;


    public WrldMap wrldMap;
    public static LatLongAltitude latLongAlt;
   

    public SteamVR_Action_Boolean touchPadTouch;
    public SteamVR_Action_Boolean touchPadPress;

    public SteamVR_Action_Vector2 touchpadAxis;

    protected float currentSpeed = 0f;

    public SteamVR_Input_Sources leftOrRightHand;

    Vector2 deadZone = new Vector2(0.2f, 0.2f);

    public bool isHeightOnly = false;

    public Transform PlayerTransform;

    private void Start()
    {
        if (!isHeightOnly)
        {
            latLongAlt = Api.Instance.SpacesApi.WorldToGeographicPoint(PlayerTransform.position);// new LatLongAltitude(37.7858, -122.401, 0);
            Api.Instance.SetOriginPoint(latLongAlt);
        }

    }

    protected void Process( bool currentlyFalling, bool modifierActive)
    {
        if (isHeightOnly)
        {
            Vector2 axisDirection = Vector2.up * touchpadAxis.GetAxis(leftOrRightHand).y ;
            currentSpeed = CalculateSpeed(axisDirection.magnitude, currentlyFalling, modifierActive);
            Move(axisDirection);
        }
        else { 
            Vector2 axisDirection = touchpadAxis.GetAxis(leftOrRightHand);
            currentSpeed = CalculateSpeed(axisDirection.magnitude, currentlyFalling, modifierActive);
            Move(axisDirection);
        }
    }

    protected void OnEnable()
    {
    }

    protected virtual float CalculateSpeed(float inputValue, bool currentlyFalling, bool modifierActive)
    {
        float speed = currentSpeed;
        if (inputValue != 0f)
        {
            speed = (maximumSpeed * inputValue);
            speed = (modifierActive ? (speed * speedMultiplier) : speed);
        }
        else
        {
            speed = Decelerate(speed, currentlyFalling);
        }

        return speed;
    }

    protected virtual float Decelerate(float speed, bool currentlyFalling)
    {
        float decelerationSpeed = (currentlyFalling ? fallingDeceleration : deceleration);
        if (speed > 0)
        {
            speed -= Mathf.Lerp(decelerationSpeed, maximumSpeed, 0f);
        }
        else if (speed < 0)
        {
            speed += Mathf.Lerp(decelerationSpeed, -maximumSpeed, 0f);
        }
        else
        {
            speed = 0;
        }

        if (speed < decelerationSpeed && speed > -decelerationSpeed)
        {
            speed = 0;
        }

        return speed;
    }

    private void Update()
    {
        if (touchPadTouch.GetActive(leftOrRightHand))
        {
            Process(false, false);
        }
    }
    protected virtual void Move(Vector2 axisDirection)
    {
        if (isHeightOnly)
        {
            Debug.Log(axisDirection);

            Vector2 updatedPosition = axisDirection * currentSpeed * Time.deltaTime * EarthLatLongMultiplier;
            var alt = latLongAlt.GetAltitude();
            alt += updatedPosition.y;
            Debug.Log(axisDirection);

            latLongAlt.SetAltitude(alt);
        }
        else { 

            Vector2 updatedPosition = axisDirection * currentSpeed * Time.deltaTime * EarthLatLongMultiplier;

            var lng = latLongAlt.GetLongitude();
            lng += updatedPosition.x;

            var lat = latLongAlt.GetLatitude();
            lat += updatedPosition.y;

            latLongAlt.SetLongitude(lng);
            latLongAlt.SetLatitude(lat);
        }

        Api.Instance.SetOriginPoint(latLongAlt);

        //if (CanMove(bodyPhysics, controlledGameObject.transform.position, finalPosition))
        //{
        //    controlledGameObject.transform.position = finalPosition;
        //}

    }
}
