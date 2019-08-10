using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class NodeDataReader : MonoBehaviour
{

    NodeData selectedNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNodeSelectEnter(object sender, DestinationMarkerEventArgs e)
    {
        Transform t = e.target;

        var nodeData = t.GetComponent<NodeData>();     

        if (nodeData != null)
        {
            if (selectedNode != null)
            {
                selectedNode.Selected = true;
            }
        }
    }
}
