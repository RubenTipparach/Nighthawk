using Hover.Core.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercastDataController : MonoBehaviour
{

    public HoverItem scanningMessage;

    public Transform rootNode;

    public HoverItem serviceListTemplate;

    public HoverItem hackingMessage;

    List<HoverItem> trackedHoverItems;
    // Start is called before the first frame update
    void Start()
    {
        trackedHoverItems = new List<HoverItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateServiceUI(List<Services> scannedServices)
    {

        if (scannedServices == null)
        {
            Debug.Log("Stuff broke, merp.");
            return;
        }

        scanningMessage.gameObject.SetActive(false);
        // dispose items
        CleanUpListItem();

        // create new list
        foreach (var s in scannedServices)
        {
            HoverItem h = GameObject.Instantiate(serviceListTemplate, rootNode);
            h.Data.Label = $"{s.name} :{s.portNum.ToString()}";
            h.gameObject.SetActive(true);
            trackedHoverItems.Add(h);
        }
    }

    public void CleanUpListItem()
    {
        foreach (var s in trackedHoverItems)
        {
            Destroy(s.gameObject);
        }

        trackedHoverItems = new List<HoverItem>();
    }
}
