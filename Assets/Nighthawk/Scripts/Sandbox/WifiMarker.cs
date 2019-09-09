using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WifiMarker : MonoBehaviour
{

    [SerializeField]
    public WifiMarkerData markerData;

    public Transform Camera;

    public TextMeshPro textMeshPro;

    public SpriteRenderer wifiSignalDisplay;

    [SerializeField]
    Sprite[] signalSprites;

    public void EnabledWifiSignaler()
    {
        wifiSignalDisplay.sprite = signalSprites[markerData.strengthLevel];
        wifiSignalDisplay.enabled = true;
    }

    public void DisableWifiSignaler()
    {
        wifiSignalDisplay.enabled = false;
    }


    public void SetTextName()
    {
        textMeshPro.text = markerData.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class WifiMarkerData
{
    public float Latitude;

    public float Longitude;

    public float Altitude;

    public string name;

    public int strengthLevel;
}
