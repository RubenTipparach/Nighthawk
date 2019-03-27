using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpatialSearchInterface : MonoBehaviour
{
    [SerializeField] public int oc1 = -1;

    [SerializeField] public int oc2 = -1;

    [SerializeField] public int oc3 = -1;

    [SerializeField] public int oc4 = -1;

    private void LateUpdate()
    {
    
    }

    List<RouterNodeTracker> trackingRouters = new List<RouterNodeTracker>();

    public bool triggerUpdate = false;

    [SerializeField] LoadNetworkData loadNetworkData;
    [SerializeField] LoadNetworkDataV2 loadNetworkDataV2;

    [SerializeField] FilterSearchMode filterMode;

    [SerializeField]
    GraphStructureV1 graphDescriptor;

    public void ClearSearch(){
       // -1 values are all wild cards.
        oc1 = -1; oc2 = -1; oc3 = -1; oc4 = -1;

        ResetAllNodes();
    }

    void ResetAllNodes()
    {
        int[] searchArray = new int[] { oc1, oc2, oc3, oc4 };
        bool searched = false;
        //trackingRouters;
        for (int i = 0; i < 4; i++)
        {
            // switch search/filter modes.
            if (searchArray[i] != -1)
            {
                searched = true;
            }
        }

        // reset everything to default!
        if (!searched)
        {
            var hngo = loadNetworkDataV2.Nodes;
            var rtNodes = hngo.Where(p => p.deviceType == GraphStructureV1.BROADBAND_ROUTER);

            foreach (var r in rtNodes)
            {
                r.AssignedGameObject.SetActive(true);
                RouterNodeTracker rnt = r.AssignedGameObject.GetComponent<RouterNodeTracker>();
                rnt.gameObject.GetComponent<LineRenderer>().enabled = false;
                rnt.SetNewPosiiton(rnt.InitPos);
                foreach (var p in rnt.childrenLines)
                {
                    p.gameObject.SetActive(true);
                }
                foreach (var p in rnt.connectionLines)
                {
                    p.gameObject.SetActive(true);
                }
            }

            var pcNodes = hngo.Where(p => p.deviceType == GraphStructureV1.GENERAL_PURPOSE);
            foreach (var h in pcNodes)
            {
                h.AssignedGameObject.SetActive(true);
            }
        }
        else
        {
            //begin the search!
            UpdateNetworkRender();
        }
    }

    public void UpdateNetworkRender()
    {
        IEnumerable<HostNode2> hngo = null;

        //if (loadNetworkData != null)
        //{
        //    hngo = loadNetworkData.Nodes;
        //}

        if (loadNetworkDataV2 != null)
        {
            hngo = loadNetworkDataV2.Nodes;
        }

        if (hngo != null)
        {
                Debug.Log("Scanning nodes in map.");
                int[] searchArray = new int[] { oc1, oc2, oc3, oc4 };

                SwitchFilterModes(hngo, searchArray);
        }
    }

    void SearchAll(HostNode2 h, int[] searchArray)
    {
        for (int i = 0; i < 4; i++)
        {
            // switch search/filter modes.
            if (searchArray[i] != -1)
            {
                if (h.OctetInterface[i] != searchArray[i])
                {
                    h.AssignedGameObject.SetActive(false);
                }
                else
                {
                    h.AssignedGameObject.SetActive(true);
                }

            }
        }
    }

    void SearchPCNode(HostNode2 h, int[] searchArray, PCNodeTracker pcNode)
    {
        for (int i = 0; i < 4; i++)
        {
            // switch search/filter modes.
            if (searchArray[i] != -1)
            {
                if (h.OctetInterface[i] != searchArray[i])
                {
                    h.AssignedGameObject.SetActive(false);
                }
                else
                {
                    h.AssignedGameObject.SetActive(true);
                    pcNode.hostNodeObj.gameObject.SetActive(true);
                    pcNode.hostNodeObj.gameObject.GetComponent<LineRenderer>().enabled = true;
                    var rtn = pcNode.hostNodeObj.gameObject.GetComponent<RouterNodeTracker>();
                    rtn.childrenLines.Where(p =>p.childIdnex == pcNode.index).FirstOrDefault().gameObject.SetActive(true);
                    trackingRouters.Add(rtn);
                }
            }
        }


        int k = 0;
        float spreadInc = 30f/ trackingRouters.Count ; //30 degree spread.
        foreach(var r in trackingRouters)
        {
            Vector3 newPosition = Quaternion.Euler(0, spreadInc*k, 0) * Vector3.forward * graphDescriptor.RouterDistanceVal;
            r.SetNewPosiiton(newPosition);
            k++;
        }
    }

    void SwitchFilterModes(IEnumerable<HostNode2> hngo,  int[] searchArray)
    {

        if (filterMode == FilterSearchMode.All)
        {
            foreach(var h in hngo) {
                SearchAll(h, searchArray);
            }
        }

        if (filterMode == FilterSearchMode.PCNodeOnly)
        {
            var rtNodes = hngo.Where(p => p.deviceType == GraphStructureV1.BROADBAND_ROUTER);
            //disable all uneeded nodes.
            foreach(var r in rtNodes) {
                r.AssignedGameObject.SetActive(false);
                RouterNodeTracker rnt = r.AssignedGameObject.GetComponent<RouterNodeTracker>();
                foreach ( var p in rnt.childrenLines)
                {
                    p.gameObject.SetActive(false);
                }

                foreach (var p in rnt.connectionLines)
                {
                    p.gameObject.SetActive(false);
                }
            }

            var pcNodes = hngo.Where(p => p.deviceType == GraphStructureV1.GENERAL_PURPOSE);
            foreach (var h in pcNodes)
            {
                var pcNode = h.AssignedGameObject.GetComponent<PCNodeTracker>();
                SearchPCNode(h, searchArray, pcNode);
            }
        }

        if (filterMode == FilterSearchMode.RouterNodeOnly)
        {
            var rtNodes = hngo.Where(p => p.deviceType == GraphStructureV1.BROADBAND_ROUTER);
            foreach (var h in rtNodes)
            {
                var routerNode = h.AssignedGameObject.GetComponent<RouterNodeTracker>();
                SearchAll(h, searchArray);
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
            trackingRouters.Clear();
            ResetAllNodes();
        

            triggerUpdate = false;
        }
    }
}

public enum FilterSearchMode
{
    All,
    PCNodeOnly,
    RouterNodeOnly
}