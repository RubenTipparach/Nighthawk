using ProceduralToolkit.Examples.Primitives;
using System.Collections;
using System.Collections.Generic;
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

            go.transform.localPosition = new Vector3(l1n.octets[2] - 128, hexahdron.height/2f, l1n.octets[3] - 128);

          
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
