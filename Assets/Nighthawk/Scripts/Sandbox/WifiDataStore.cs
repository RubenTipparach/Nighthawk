using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WifiDataStore", menuName = "ScriptableObjects/WifiDataStore", order = 1)]
public class WifiDataStore : ScriptableObject
{
    [SerializeField]
    public List<WifiMarkerData> markerData;
}
