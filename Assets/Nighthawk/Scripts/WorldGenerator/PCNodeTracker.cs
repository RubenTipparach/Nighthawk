using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCNodeTracker : MonoBehaviour
{

    private HostNode2 _hostNode;

    public HostNode2 HostNode
    {
        get
        {
            return _hostNode;
        }
        set
        {
            _hostNode = value;
            hostNodeObj = value.AssignedGameObject.transform;
        }
    }

    public int index;

    public Transform hostNodeObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hostNodeObj != null)
        {
            var angleAxis = (hostNodeObj.position - GraphStructureV1.CenterGrid).normalized;
            Vector3 rotationOffset = Quaternion.LookRotation(angleAxis) * Quaternion.Euler(0, 0, 30 * index) * Vector3.right * 16;
            var adjNodePosition = hostNodeObj.position + angleAxis * 30 + rotationOffset;//+ Vector3.right *index *10;//Quaternion.Euler(0, 0, 30 * index) ;
            transform.position = adjNodePosition;
        }
    }
}
