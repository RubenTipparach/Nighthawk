using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    public HostNode2 nodeInfo;

    // Todo: add to json file.
    public NodeDataChunk nodeDataChunk;

    public TextMeshPro textMeshPro;

    [SerializeField]
    bool _selected = false;

    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;

            SelectionSystem.gameObject.SetActive(_selected);
        }
    }

    [SerializeField]
    Transform SelectionSystem;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = nodeDataChunk.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class NodeDataChunk
{
    public string Name;
}