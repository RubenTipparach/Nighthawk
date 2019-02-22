using ProceduralToolkit.Examples.Primitives;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IPCityScape : MonoBehaviour
{

    [SerializeField]
    LoadNetworkData loadedNetworkData;

    HostNode[] nodes;

    [SerializeField]
    Hexahedron buildingPrefab;

    [SerializeField]
    Material Lvl1_Host_Mat;

    [SerializeField]
    Material Lvl2_Host_Mat;

    [SerializeField]
    TextMeshPro textPrefab;

    private void Awake()
    {
        loadedNetworkData.finishedLoadingData += LoadedNetworkData_finishedLoadingData;
    }

    private void LoadedNetworkData_finishedLoadingData(LoadingNetworkDataArgs args, object sender)
    {
        nodes = args.HDP.data;

        foreach(var l1n in nodes)
        {
            var go = Instantiate(buildingPrefab.gameObject, transform);
            var hexahdron = go.GetComponent<Hexahedron>();
            go.GetComponent<MeshRenderer>().material = Lvl1_Host_Mat;

            l1n.AssignedGameObject = go;

            hexahdron.length = 1;
            hexahdron.width = 1;

            if (l1n.child.Length != 0)
            {
                hexahdron.height = l1n.child.Length + 1;
            }
            else
            {
                hexahdron.height = 1;
            }
            
            var offsetHeight = hexahdron.height / 2f;
            go.transform.localPosition = new Vector3(l1n.octets[2] - 128, offsetHeight, l1n.octets[3] - 128);

            // set text position
            var textObj = Instantiate(textPrefab, go.transform);
            textObj.transform.localPosition = new Vector3(0, offsetHeight + 1, 0);

            var tmp = textObj.GetComponent<TextMeshPro>();
            tmp.SetText($"{l1n.octets[0]}.{l1n.octets[1]}.{l1n.octets[2]}.{l1n.octets[3]}");

            float index = 0;
            foreach (var l2n in l1n.child)
            {
                l2n.AssignedGameObject = go;

                var go2 = Instantiate(buildingPrefab.gameObject, go.transform);
                var hexahdron2 = go2.GetComponent<Hexahedron>();
                go2.GetComponent<MeshRenderer>().material = Lvl2_Host_Mat;

                hexahdron2.length = 0.8f;
                hexahdron2.width = 0.8f;
                hexahdron2.height = 0.4f;

                go2.transform.localPosition = new Vector3(0, index/1.2f - offsetHeight/1.2f, 0);

                // set text position
                var textObj2 = Instantiate(textPrefab, go2.transform);
                textObj2.transform.localPosition = new Vector3(1.6f, 0, 0);
                textObj2.transform.localScale = Vector3.one;

                // set text content
                var tmp2 = textObj2.GetComponent<TextMeshPro>();
                tmp2.SetText($"{l2n.octets[0]}.{l2n.octets[1]}.{l2n.octets[2]}.{l2n.octets[3]}");


                index++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
