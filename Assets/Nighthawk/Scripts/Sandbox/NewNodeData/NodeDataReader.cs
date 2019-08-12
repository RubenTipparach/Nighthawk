using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class NodeDataReader : MonoBehaviour
{

    NodeData selectedNode;

    [SerializeField]
    Text unityText;

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
                selectedNode.Selected = false;
                selectedNode = null;
            }

            nodeData.Selected = true;
            selectedNode = nodeData;
            unityText.text = nodeData.nodeDataChunk.Name;
        }
    }
}
