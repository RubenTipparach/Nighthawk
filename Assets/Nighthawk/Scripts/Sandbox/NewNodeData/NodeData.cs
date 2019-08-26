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

    public TextMeshPro stateText;

    [SerializeField]
    bool _selected = false;

    float currentTimePassed;


    public Action onFinishedStateCallback;

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
            else
            {
                currentNodeState = NodeDataState.Scanned;
                stateText.text = currentNodeState.ToString();
                progressBar.fillAmount = 0;
                // callback on the state transition, and set it back to idle.
                onFinishedStateCallback?.Invoke();
                onFinishedStateCallback = null;
            }

        }
        // if node is in scanning mode, start spinning the icon.
    }


    public void BeginScan(Action onFinished = null)
    {
        progressBar.fillAmount = 0;
        currentNodeState = NodeDataState.ScanningNode;
        stateText.text = currentNodeState.ToString();

        onFinishedStateCallback = onFinished;
    }

    public enum NodeDataState
    {
        Idle,
        ScanningNode,
        Scanned,
        BeingHacked,
        Hacked,
        ShuttingDown,
        Offline
    }
}

[Serializable]
public class NodeDataChunk
{
    public string Name;

    public List<Services> services;
}

[Serializable]
public class Services
{
    public string name;

    public string description;

    public int portNum;

}