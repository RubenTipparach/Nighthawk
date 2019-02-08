using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadNetworkData : MonoBehaviour
{
    [SerializeField]
    private string hostAddress = "http://localhost:3001/";

    public static string SERVER_HOST
    {
        get;
        private set;
    }

    public HostNode[] Nodes
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        SERVER_HOST = hostAddress;

        StartCoroutine(getNodes());
    }

    IEnumerator getNodes()
    {
        // get IP address data
        UnityWebRequest req = UnityWebRequest.Get($"{SERVER_HOST}get/ipAddresses");

        yield return req.SendWebRequest();

        if(req.isNetworkError || req.isHttpError)
        {
            Debug.LogWarning(req.error);
        }
        else
        {
            string res = req.downloadHandler.text;
            HostDataPackage hdp = JsonUtility.FromJson<HostDataPackage>(res);
            //foreach ( var n in hdp.data)
            //{
            //   // Debug.Log($"{n.octets[0]}.{n.octets[1]}.{n.octets[2]}.{n.octets[3]}");
            //}

            finishedLoadingData(new LoadingNetworkDataArgs(hdp), this);
        }
    }

    public event FinishedLoadingData finishedLoadingData;

    // Update is called once per frame
    void Update()
    {
        
    }
}

public delegate void FinishedLoadingData(LoadingNetworkDataArgs args, object sender);

public class LoadingNetworkDataArgs : EventArgs
{
    public HostDataPackage HDP { get; private set; }

    public LoadingNetworkDataArgs(HostDataPackage hdp) : base()
    {
        this.HDP = hdp;
    }
}

[Serializable]
public class HostDataPackage
{
    public HostNode[] data;
}

// Serializable class for IP nodes.
[Serializable]
public class HostNode
{
    public int[] octets;
    public HostNode[] child;
}