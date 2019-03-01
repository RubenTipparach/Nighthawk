using ProceduralToolkit.Examples.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphStructureV1 : MonoBehaviour
{

    [SerializeField]
    LoadNetworkDataV2 loadNetworkDataV2;

    [SerializeField]
    Octahedron nodePrefab;

    [SerializeField]
    Material lineMat;

    [SerializeField]
    Material lineMat2;

    [SerializeField]
    Material nodeMat;

    [SerializeField]
    TextMeshPro textPrefab;

    [SerializeField]
    LineRenderer linePrefab;

    private HostNode2[] nodes;

    private void Awake()
    {
        loadNetworkDataV2.finishedLoadingData += LoadedNetworkData_finishedLoadingData;
    }

    private Vector3 getLocationFromOctet(int[] octets)
    {
        return new Vector3((float)octets[2] / 2f - 128f, (float)octets[1] / 2f, (float)octets[3] / 2f - 128f);
    }

    public readonly Vector3 centerGrid = new Vector3(0, 0, 0);

    public const string BROADBAND_ROUTER = "Broadband Router";
    public const string GENERAL_PURPOSE = "General Purpose";

    private void LoadedNetworkData_finishedLoadingData(LoadingNetworkDataArgs args, object sender)
    {
        nodes = args.HDP2.data;

        // preprocess all GP nodes into childs of BR nodes
        foreach (var GP in nodes)
        {
            if (GP.deviceType == GENERAL_PURPOSE)
            {
                foreach (var BRN in GP.connections)
                {
                    var BR = nodes[BRN];

                    if (BR.deviceType == BROADBAND_ROUTER)
                    {
                        if (!BR.connections.Contains(GP.id))
                        {
                            BR.connections = BR.connections.Append(GP.id).ToArray();
                        }
                    }
                }

                GP.connections = new int[0];
            }
        }

        foreach (var l1n in nodes)
        {
            var go = Instantiate(nodePrefab.gameObject, transform);
            var octahedron = go.GetComponent<Octahedron>();
            go.GetComponent<MeshRenderer>().material = nodeMat;
            octahedron.radius = 1;

            //what to do with octet 0?
            var nodePosition = getLocationFromOctet(l1n.octets);
            go.transform.localPosition = nodePosition;

            // set text position
            var textObj = Instantiate(textPrefab, go.transform);
            textObj.transform.localPosition = new Vector3(0, 1, 0);

            var tmp = textObj.GetComponent<TextMeshPro>();
            tmp.SetText($" {l1n.deviceType} |  {l1n.octets[0]}.{l1n.octets[1]}.{l1n.octets[2]}.{l1n.octets[3]}");


            l1n.AssignedGameObject = go;

        }

        // finally create all the connections
        foreach (var l1n in nodes)
        {
            float index = 0;
            var nodePosition = l1n.AssignedGameObject.transform.position;

            // All l2n should be GP now.
            foreach (var l2n in l1n.connections)
            {
                var go2 = Instantiate(linePrefab.gameObject, l1n.AssignedGameObject.transform);
                HostNode2 adjNode = nodes[l2n];

                //go2.GetComponent<MeshRenderer>().material = Lvl2_Host_Mat;
                LineRenderer lr = go2.GetComponent<LineRenderer>();

                if (adjNode.deviceType == BROADBAND_ROUTER && l1n.deviceType == BROADBAND_ROUTER)
                {
                    lr.sharedMaterial = lineMat2;
                }

                if (adjNode.deviceType == GENERAL_PURPOSE && l1n.deviceType == BROADBAND_ROUTER)
                {
                    // change 4 to some arbitrary value.
                    var angleAxis = (nodePosition - centerGrid).normalized;
                   

                    var adjNodePosition = nodePosition + Quaternion.Euler(0, 0, 30 * index) * angleAxis * 20 ;
                    adjNode.AssignedGameObject.transform.position = adjNodePosition;
                    lr.SetPositions(new Vector3[] { nodePosition, adjNodePosition });
                }
                else
                {
                    var adjNodePosition = getLocationFromOctet(adjNode.octets);
                    lr.SetPositions(new Vector3[] { nodePosition, adjNodePosition });
                }

                index++;
            }
        }
    }
}
