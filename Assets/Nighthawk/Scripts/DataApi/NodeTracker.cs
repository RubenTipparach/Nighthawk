using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTracker : MonoBehaviour
{
    public GameObject HostNode;
    public GameObject ConnectionNode;

    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(HostNode != null && ConnectionNode != null)
        {
            lr.SetPositions(new Vector3[] { HostNode.transform.position, ConnectionNode.transform.position });
        }
    }
}
