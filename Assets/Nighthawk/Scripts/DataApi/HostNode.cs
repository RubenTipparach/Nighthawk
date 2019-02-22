using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class HostDataPackage
{
    public HostNode[] data;
}

// Serializable class for IP nodes.
[Serializable]
public class HostNode : HostNodeGameOject
{
    public int[] octets;
    public HostNode[] child;

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

