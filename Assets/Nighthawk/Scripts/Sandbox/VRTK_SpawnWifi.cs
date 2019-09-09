using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Wrld;
using Wrld.Space;

public class VRTK_SpawnWifi : MonoBehaviour
{

    public SteamVR_Action_Boolean button1Press;

    public SteamVR_Action_Boolean button2Press;


    public GameObject spawnWifiObj;

    public GameObject player;

    public SteamVR_Input_Sources leftOrRightHand;

    public WFSpawnMode spawnMode;

    public WifiDataStore dataStore;

    public GameObject spawnGenericMarker;

    public enum WFSpawnMode
    {
        Editor, Auto
    }

    List<WifiMarker> wifiMarkers;

    bool markersEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        wifiMarkers = new List<WifiMarker>();

        if(dataStore.markerData == null)
        {
            dataStore.markerData = new List<WifiMarkerData>();
        }
        // todo add manually here to set up scenario.

        if (spawnMode == WFSpawnMode.Auto)
        {
            foreach (var loadedMarker in dataStore.markerData)
            {
                //var latLongAlt = Api.Instance.SpacesApi.WorldToGeographicPoint(player.transform.position);

                LatLongAltitude latLongAlt = new LatLongAltitude(loadedMarker.Latitude, loadedMarker.Longitude, loadedMarker.Altitude);

                var wifi = Instantiate(spawnWifiObj) as GameObject;
                wifi.GetComponent<GeographicTransform>().SetPosition(latLongAlt.GetLatLong());
                wifi.GetComponent<GeographicTransform>().SetElevation(latLongAlt.GetAltitude());

                var marker = wifi.GetComponent<WifiMarker>();
                marker.markerData.Latitude = (float)latLongAlt.GetLatitude();
                marker.markerData.Longitude = (float)latLongAlt.GetLongitude();
                marker.markerData.Altitude = (float)latLongAlt.GetAltitude();
                marker.markerData.name = loadedMarker.name;
                marker.markerData.strengthLevel = loadedMarker.strengthLevel;
                marker.EnabledWifiSignaler();
                marker.SetTextName();

                wifiMarkers.Add(marker);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if(spawnMode == WFSpawnMode.Editor && button1Press.GetStateDown(leftOrRightHand))
        {

            Debug.Log("Spawn Wifi Pressed");
            var latLongAlt = Api.Instance.SpacesApi.WorldToGeographicPoint(player.transform.position);

            var wifi = Instantiate(spawnWifiObj) as GameObject;
            wifi.GetComponent<GeographicTransform>().SetPosition(latLongAlt.GetLatLong());
            wifi.GetComponent<GeographicTransform>().SetElevation(latLongAlt.GetAltitude());

            var marker = wifi.GetComponent<WifiMarker>();
            marker.markerData.Latitude = (float)latLongAlt.GetLatitude();
            marker.markerData.Longitude= (float)latLongAlt.GetLongitude();
            marker.markerData.Altitude = (float)latLongAlt.GetAltitude();
            marker.markerData.name = "marker " + wifiMarkers.Count;
            
            marker.EnabledWifiSignaler();
            marker.SetTextName();

            wifiMarkers.Add(marker);
            dataStore.markerData.Add(marker.markerData);
            //Api.Instance.SetOriginPoint(latLongAlt);

        }


        if (spawnMode == WFSpawnMode.Auto && button1Press.GetStateDown(leftOrRightHand))
        {
            var latLongAlt = Api.Instance.SpacesApi.WorldToGeographicPoint(player.transform.position);
            var genMarker = Instantiate(spawnGenericMarker) as GameObject;
            genMarker.GetComponent<GeographicTransform>().SetPosition(latLongAlt.GetLatLong());
            genMarker.GetComponent<GeographicTransform>().SetElevation(latLongAlt.GetAltitude());
        }

        if (button2Press.GetStateDown(leftOrRightHand))
        {
            foreach (var marker in wifiMarkers)
            {
                markersEnabled = !markersEnabled;

                if(markersEnabled)
                {
                    marker.EnabledWifiSignaler();
                }
                else
                {
                    marker.DisableWifiSignaler();
                }
            }
        }

    }
}
