using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class HostDataPackage2
{
    public HostNode2[] data;
}

// Serializable class for IP nodes.
[Serializable]
public class HostNode2 : HostNodeGameOject
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

    private GameObject _ago;
    public GameObject AssignedGameObject
    {
        get { return _ago; }
        set { _ago = value; }
    }

    public int[] OctetInterface
    {
        get { return octets; }
    }
}