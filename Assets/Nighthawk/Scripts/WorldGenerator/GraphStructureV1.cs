using ProceduralToolkit.Examples.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GraphStructureV1 : MonoBehaviour
{
    public Camera playerCamera;

    [SerializeField]
    LoadNetworkDataV2 loadNetworkDataV2;

    [SerializeField]
    Octahedron nodePrefab;

    [SerializeField]
    Material lineMat;

    [SerializeField]
    Material lineMat2;

    [SerializeField]
    Material searchLineMat;

    [SerializeField]
    Material nodeMat;

    [SerializeField]
    TextMeshPro textPrefab;

    [SerializeField]
    LineRenderer linePrefab;

    private HostNode2[] nodes;

    public int seed = 42;

    [SerializeField]
    Transform Routers;

    [SerializeField]
    Transform PCs;

    [SerializeField]
    public Transform CenterTransform;

    private void Awake()
    {
        loadNetworkDataV2.finishedLoadingData += LoadedNetworkData_finishedLoadingData;
        sCenterTransform = CenterTransform;
    }

    // Generates routers in a cluster
    private Vector3 getLocationFromOctet(int[] octets)
    {
        return new Vector3((float)octets[2] / 2f - 128f, (float)octets[1] / 2f, (float)octets[3] / 2f - 128f);
    }

    public float RouterDistanceVal = 64;

    private Vector3 getFromRandomSphere()
    {
        return Random.onUnitSphere* 64;
    }

    static Transform sCenterTransform;

    public static Vector3 CenterGrid
    {
        get { return sCenterTransform.position; }
    }

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

        Random.InitState(42);

        // places router stuff first!// it also places all nodes in some default position.
        foreach (var l1n in nodes)
        {
            //string name;

            var go = Instantiate(nodePrefab.gameObject);

            if (l1n.deviceType == GENERAL_PURPOSE)
            {
                go.transform.SetParent( PCs);
                go.AddComponent<PCNodeTracker>();
            }
            if (l1n.deviceType == BROADBAND_ROUTER)
            {
                go.transform.SetParent(Routers);
            }

            
            var octahedron = go.GetComponent<Octahedron>();
            go.GetComponent<MeshRenderer>().material = nodeMat;
            octahedron.radius = 1;

            //what to do with octet 0?
            //var nodePosition = getLocationFromOctet(l1n.octets) * 2;
            var nodePosition = getFromRandomSphere();
            go.transform.localPosition = nodePosition;

            // set text position
            var textObj = Instantiate(textPrefab, go.transform);
            textObj.transform.localPosition = new Vector3(0, 1, 0);

            var tmp = textObj.GetComponent<TextMeshPro>();
            tmp.SetText($" {l1n.deviceType} |  {l1n.octets[0]}.{l1n.octets[1]}.{l1n.octets[2]}.{l1n.octets[3]}");

            // set to follow the camera's movement
            var script = textObj.GetComponent<FaceCamera>();
            script.cameraToLookAt = playerCamera;


            l1n.AssignedGameObject = go;

        }

        // finally create all the connections
        //return;
        foreach (var l1n in nodes)
        {
            int index = 0;
            var nodePosition = l1n.AssignedGameObject.transform.position;
            var rtNode = l1n.AssignedGameObject.AddComponent<RouterNodeTracker>();

            LineRenderer hiddenLr = l1n.AssignedGameObject.AddComponent<LineRenderer>();
            hiddenLr.enabled = false;
            hiddenLr.sharedMaterial = searchLineMat;
            NodeTracker hiddenNt = l1n.AssignedGameObject.AddComponent<NodeTracker>();
            hiddenNt.HostNode = l1n.AssignedGameObject;
            hiddenNt.ConnectionNode = CenterTransform.gameObject; // TODO: update this to whatever you want to pivot on search.

            // skip generals
            //if (l1n.deviceType == GENERAL_PURPOSE) continue;


            // All l2n should be GP now.
            foreach (var l2n in l1n.connections)
            {
                var go2 = Instantiate(linePrefab.gameObject, l1n.AssignedGameObject.transform);
                HostNode2 adjNode = nodes[l2n];

                //go2.GetComponent<MeshRenderer>().material = Lvl2_Host_Mat;
                LineRenderer lr = go2.GetComponent<LineRenderer>();

                // added node tracker to maintain the position of the nodes.
                NodeTracker nt = go2.AddComponent<NodeTracker>();
                nt.HostNode = l1n.AssignedGameObject;
                nt.ConnectionNode = nodes[l2n].AssignedGameObject;

                // both are broadband types
                if (adjNode.deviceType == BROADBAND_ROUTER && l1n.deviceType == BROADBAND_ROUTER)
                {
                    lr.sharedMaterial = lineMat2;
                    rtNode.connectionLines.Add(nt);
                }
                
                // a pc node is adjacent, assign it as child.
                if (adjNode.deviceType == GENERAL_PURPOSE && l1n.deviceType == BROADBAND_ROUTER)
                {
                    // Since I'm a PC node, I'm going to have this system override stuff.
                    var pcnt = adjNode.AssignedGameObject.GetComponent<PCNodeTracker>();
                    pcnt.HostNode = l1n;
                    rtNode.childrenNodes.Add(adjNode);
                    pcnt.index = index;
                    nt.childIdnex = index;
                    rtNode.childrenLines.Add(nt);
                }                

                index++;
            }
        }
    }
}
