using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchIPs : MonoBehaviour
{

    public GameObject searchInterface;
    private SpatialSearchInterface searchScript;

    // Start is called before the first frame update
    void Start()
    {
        searchScript = searchInterface.GetComponent<SpatialSearchInterface>();
    }

    public void startSearch(string ip)
    {
        string[] splitIP = ip.Split('.');
        int[] results = new int[4];
        for ( int i = 0; i < splitIP.Length; i++)
        {
            if (splitIP[i] == "")
                results[i] = -1;
            else
                results[i] = int.Parse(splitIP[i]);

        }

        searchScript.oc1 = results[0];
        searchScript.oc2 = results[1];
        searchScript.oc3 = results[2];
        searchScript.oc4 = results[3];
        
        searchScript.triggerUpdate = true;
    }
}
