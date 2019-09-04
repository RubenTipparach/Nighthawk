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

    public SecurityLevel securityLevel;

    // Todo: add to json file.
    public NodeDataChunk nodeDataChunk;

    public TextMeshPro textMeshPro;

    public TextMeshPro stateText;

    [SerializeField]
    bool _selected = false;

    float currentTimePassed;
    
    public Action onFinishedStateCallback;

    [SerializeField]
    NodeMatMapper matMapper;

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
        stateText.text = nameof(currentNodeState);        
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
            else if(currentNodeState == NodeDataState.ScanningNode)
            {
                UpdateNodeState(NodeDataState.Scanned);

                // callback on the state transition, and set it back to idle.
                onFinishedStateCallback?.Invoke();
                onFinishedStateCallback = null;
            }
            else if (currentNodeState == NodeDataState.BeingHacked)
            {
                if(securityLevel == SecurityLevel.NotHackable)
                {
                    UpdateNodeState(NodeDataState.HackFailed); ;
                }
                else
                {
                    UpdateNodeState(NodeDataState.Hacked);
                }

                onFinishedStateCallback?.Invoke();
                onFinishedStateCallback = null;
            }
        }
        // if node is in scanning mode, start spinning the icon.
    }


    public void BeginScan(Action onFinished = null)
    {
        // reset timer...
        currentTimePassed = 0;
        UpdateNodeState(NodeDataState.ScanningNode);

        onFinishedStateCallback = onFinished;
    }

    public void BeginHack(Action onFinished = null)
    {
        currentTimePassed = 0;

        UpdateNodeState(NodeDataState.BeingHacked);

        onFinishedStateCallback = onFinished;
    }

    private void UpdateNodeState(NodeDataState nodeDataState, bool changeColor = true)
    {
        currentNodeState = nodeDataState;
        progressBar.fillAmount = 0;
        stateText.text = currentNodeState.ToString();

        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (!changeColor) return;

        switch (nodeDataState)
        {
            case NodeDataState.Idle:
                mr.material = matMapper.Neutral;
                break;
            case NodeDataState.Hacked:
                mr.material = matMapper.Captured;
                break;
            case NodeDataState.Scanned:
                mr.material = matMapper.Scanned;
                break;
            case NodeDataState.HackFailed:
                mr.material = matMapper.Blocked;
                break;
            case NodeDataState.Offline:
                mr.material = matMapper.Offline;
                break;
            default:
                mr.material = matMapper.InProgress;
                break;
        }
    }

    public enum NodeDataState
    {
        Idle,
        ScanningNode,
        Scanned,
        BeingHacked,
        Hacked,
        HackFailed,
        ShuttingDown,
        Offline
    }

    public enum SecurityLevel
    {
        IsHackable,
        NotHackable
    }
}


[Serializable]
public class NodeDataChunk
{
    public string Name;

    public List<Services> services;
}

[Serializable]
public class NodeMatMapper
{
    public Material Neutral;

    public Material Captured;

    public Material Blocked;

    public Material Offline;

    public Material InProgress;

    public Material Scanned;
}

[Serializable]
public class Services
{
    public string name;

    public string description;

    public int portNum;

}