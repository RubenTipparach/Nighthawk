using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LoadNetworkDataV2 : MonoBehaviour
{
    public TextAsset txt;

    [SerializeField]
    private string hostAddress = "http://localhost:3001/";

    public bool offlineMode = true;

    public static string SERVER_HOST
    {
        get;
        private set;
    }

    public HostNode2[] Nodes
    {
        get; private set;
    }
    
    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2} ", b);
        return hex.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        SERVER_HOST = hostAddress;

        if (offlineMode)
        {
            // = (TextAsset)Resources.Load("db.json", typeof(TextAsset));

            string res = txt.text;
            HostDataPackage2 hdp = JsonUtility.FromJson<HostDataPackage2>(res);

            foreach (HostNode2 h in hdp.data )
            {
                byte[] bs = h.macAddress.Select(p => { byte b; byte.TryParse(p +"", out b); return b; }).ToArray();
                h.tMacAddress = ByteArrayToString(bs);
                Debug.Log($"{h.octets[0]}.{h.octets[1]}.{h.octets[2]}.{h.octets[3]}");
                Debug.Log(h.tMacAddress);
            }

            if (finishedLoadingData != null)
            {
                finishedLoadingData(new LoadingNetworkDataArgs(hdp), this);
            }

            // actions:
            //  - scan port
            //  - suggest possible exploits
            //      - system vulnerable to keylogger
            //      - root kit
            //      - DDOS attack on unprotected HTTP port.
        }

    }

    public event FinishedLoadingData finishedLoadingData;


    [Serializable]
    public class HostDataPackage2
    {
        public HostNode2[] data;
    }

    // Serializable class for IP nodes.
    [Serializable]
    public class HostNode2
    {
        public int id;
        public int[] octets;

        public int[] macAddress;
        public string tMacAddress;

        public int status;

        public int latency;
        public string deviceType;

        public string os;

        public int[] connections;

        public int[] ports;
    }
}

