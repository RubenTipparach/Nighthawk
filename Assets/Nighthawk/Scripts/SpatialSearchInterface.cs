using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialSearchInterface : MonoBehaviour
{
    [SerializeField] int oc1 = -1;

    [SerializeField] int oc2 = -1;

    [SerializeField] int oc3 = -1;

    [SerializeField] int oc4 = -1;


    public bool triggerUpdate = false;

    [SerializeField] LoadNetworkData loadNetworkData;
    [SerializeField] LoadNetworkDataV2 loadNetworkDataV2;

    public void ClearSearch(){
       // -1 values are all wild cards.
        oc1 = -1; oc2 = -1; oc3 = -1; oc4 = -1;

        UpdateNetworkRender();
    }

    public void UpdateNetworkRender()
    {
        IEnumerable<HostNodeGameOject> hngo = null;

        if (loadNetworkData != null)
        {
            hngo = loadNetworkData.Nodes;
        }

        if (loadNetworkDataV2 != null)
        {
            hngo = loadNetworkDataV2.Nodes;
        }

        if (hngo != null)
        {
            Debug.Log("Scanning nodes in map.");
            int[] searchArray = new int[] { oc1, oc2, oc3, oc4 };
            foreach (var h in hngo)
            {
                
                for (int i = 0; i < 4; i++)
                {
                    if (searchArray[i] != -1)
                    {
                        if(h.OctetInterface[i] != searchArray[i]){
                            h.AssignedGameObject.SetActive(false);
                        }
                        else{
                            h.AssignedGameObject.SetActive(true);
                        }
                    }
                }
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
        if(triggerUpdate)
        {
            UpdateNetworkRender();

            triggerUpdate = false;
        }
    }
}

