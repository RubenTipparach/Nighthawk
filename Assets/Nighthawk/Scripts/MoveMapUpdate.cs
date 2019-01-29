using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;

public class MoveMapUpdate : MonoBehaviour
{

    double alt;

    LatLongAltitude latLongAlt;

    bool initialized;

    // Start is called before the first frame update
    // a  really important note is that the mouse MUST be in the game in order for updates to occur!
    void Start()
    {
        StartCoroutine(lateStart());

        initialized = false;
    }

    IEnumerator lateStart()
    {
        yield return new WaitForSeconds(4.0f);

        var positionOpt = new Wrld.Space.Positioners.PositionerOptions();
        //Api.Instance.PositionerApi.CreatePositioner(positionOpt);
        latLongAlt = new LatLongAltitude(37.7858, -122.401, 0);
        // Debug.Log(currentPosition.ToString());

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            //var latlong = latLongAlt.GetLatLong();
            var lng = latLongAlt.GetLongitude();
            lng += .001 * Time.deltaTime;

            var lat = latLongAlt.GetLatitude();
            lat += 0.001 * Time.deltaTime;

            latLongAlt.SetLongitude(lng);
            latLongAlt.SetLatitude(lat);

            Api.Instance.SetOriginPoint(latLongAlt);
        }
        //Api.Instance.CameraApi.
    }
}
