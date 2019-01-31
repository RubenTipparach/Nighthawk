using BaroqueUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;

public class LatLngAltControls : MonoBehaviour
{
    public Dialog dialogSetLat;

    public Dialog dialogSetLong;

    public Dialog dialogSetAlt;


    LatLongAltitude startLatLongAlt;
    LatLongAltitude latLongAlt;

    float latitude;
    float longitude;
    float altitudeInMetres;

    private void Start()
    {
        var ct = Controller.HoverTracker(this);
        //ct.onTriggerDown += OnTriggerDown;
        var positionOpt = new Wrld.Space.Positioners.PositionerOptions();
        //Api.Instance.PositionerApi.CreatePositioner(positionOpt);

        startLatLongAlt = Api.Instance.SpacesApi.WorldToGeographicPoint(new Vector3(0, 100, 0));

        Debug.Log(startLatLongAlt.GetLatitude());
        Debug.Log(startLatLongAlt.GetLongitude());
        Debug.Log(startLatLongAlt.GetAltitude());


        latitude = 0;
        longitude = 0;
        altitudeInMetres = 0;

        latLongAlt = startLatLongAlt;

        dialogSetLat.Set("Slider", transform.rotation.eulerAngles.y, onChange: value =>
        {
            latitude = value / 9000f;
            SetPosition();

        });

        dialogSetLong.Set("Slider", transform.rotation.eulerAngles.y, onChange: value =>
        {
            longitude = value / 9000f;
            SetPosition();

        });

        dialogSetAlt.Set("Slider", transform.rotation.eulerAngles.y, onChange: value =>
        {
            altitudeInMetres = value;

            SetPosition();
        });
    }

    private void SetPosition()
    {
        latLongAlt.SetLatitude(startLatLongAlt.GetLatitude() + (double)latitude);
        latLongAlt.SetLongitude(startLatLongAlt.GetLongitude() + (double)longitude);
        latLongAlt.SetAltitude(startLatLongAlt.GetAltitude() + (double)altitudeInMetres);

        Api.Instance.SetOriginPoint(latLongAlt);
    }

}


