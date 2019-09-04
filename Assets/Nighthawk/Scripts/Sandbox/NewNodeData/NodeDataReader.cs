using System;
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

    public List<Services> scannedServices;

    public HovercastDataController hdc;

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


    public void ScanNode()
    {
        if(selectedNode != null)
        {

            hdc.scanningMessage.gameObject.SetActive(true);
            hdc.hackingMessage.gameObject.SetActive(false);

            Action callback = () =>
            {
                scannedServices = selectedNode.nodeDataChunk.services;
                hdc.GenerateServiceUI(scannedServices);
            };

            selectedNode.BeginScan(callback);
        }
    }

    public void HackNode()
    {
        if (selectedNode != null)
        {

            hdc.hackingMessage.gameObject.SetActive(true);

            Action callback = () =>
            {
                //scannedServices = selectedNode.nodeDataChunk.services;
                //hdc.GenerateServiceUI(scannedServices);
                // dunno what to do here yet...
                hdc.hackingMessage.gameObject.SetActive(false);
            };

            selectedNode.BeginHack(callback);
        }
    }
}
