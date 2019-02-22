using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadNetworkData : MonoBehaviour
{
    [SerializeField]
    private string hostAddress = "http://localhost:3001/";

    public bool offlineMode = false;

    public TextAsset txt;

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

        if (offlineMode)
        {
           // = (TextAsset)Resources.Load("db.json", typeof(TextAsset));

            string res = txt.text;
            HostDataPackage hdp = JsonUtility.FromJson<HostDataPackage>(res);
            Nodes = hdp.data;
            finishedLoadingData(new LoadingNetworkDataArgs(hdp), this);
        }
        else
        {
            StartCoroutine(getNodes());
        }
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

    public HostDataPackage2 HDP2 { get; private set; }


    public LoadingNetworkDataArgs(HostDataPackage hdp) : base()
    {
        this.HDP = hdp;
    }

    public LoadingNetworkDataArgs(HostDataPackage2 hdp) : base()
    {
        this.HDP2 = hdp;
    }

}

