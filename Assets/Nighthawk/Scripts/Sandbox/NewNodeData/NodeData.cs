using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeData : MonoBehaviour
{
    public HostNode2 nodeInfo;

    public Image progressBar;

    public NodeDataState currentNodeState;

    // Todo: add to json file.
    public NodeDataChunk nodeDataChunk;

    public TextMeshPro textMeshPro;

    [SerializeField]
    bool _selected = false;

    float currentTimePassed;

    [SerializeField]
    float timeToOperate = 2.5f;
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
        if(currentNodeState != NodeDataState.Idle)
        {
            //progressBar.imaget
            if(currentTimePassed < timeToOperate)
            {
                currentTimePassed += Time.deltaTime;
                progressBar.fillAmount = currentTimePassed / timeToOperate;
            }
            else
            {
                currentNodeState = NodeDataState.Idle;
            }

        }
        // if node is in scanning mode, start spinning the icon.
    }

    public enum NodeDataState
    {
        Idle,
        ScanningNode,
        BeingHacked,
        ShuttingDown
    }
}

[Serializable]
public class NodeDataChunk
{
    public string Name;
}