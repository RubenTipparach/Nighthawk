using ProceduralToolkit.Examples.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
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
        return new Vector3((float)octets[2] / 2f - 128f, (float)octets[1] / 2f, (float)octets[3]/2f - 128f);
    }
    private void LoadedNetworkData_finishedLoadingData(LoadingNetworkDataArgs args, object sender)
    {
        nodes = args.HDP2.data;

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
            tmp.SetText($"{l1n.octets[0]}.{l1n.octets[1]}.{l1n.octets[2]}.{l1n.octets[3]}");

            float index = 0;

            l1n.AssignedGameObject = go;

            foreach (var l2n in l1n.connections)
            {
                var go2 = Instantiate(linePrefab.gameObject, go.transform);
                HostNode2 adjNode = nodes[l2n];

                //go2.GetComponent<MeshRenderer>().material = Lvl2_Host_Mat;
                LineRenderer lr = go2.GetComponent<LineRenderer>();
                var adjNodePosition = getLocationFromOctet(adjNode.octets);

                lr.SetPositions(new Vector3[] { nodePosition, adjNodePosition });

                // go2.transform.localPosition = new Vector3(0, index / 1.2f - offsetHeight / 1.2f, 0);

                // set text position
                //var textObj2 = Instantiate(textPrefab, go2.transform);
                // textObj2.transform.localPosition = new Vector3(1.6f, 0, 0);
                //textObj2.transform.localScale = Vector3.one;
                // set text content
                // var tmp2 = textObj2.GetComponent<TextMeshPro>();
                //tmp2.SetText($"{adjNode.octets[0]}.{adjNode.octets[1]}.{adjNode.octets[2]}.{adjNode.octets[3]}");

                index++;
            }
        }
    }
}
