using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouterNodeTracker : MonoBehaviour
{

    public HostNode2 nodesConnected;

    public Transform globalCenter;

    public List<HostNode2> childrenNodes = new List<HostNode2>();

    public List<NodeTracker> childrenLines = new List<NodeTracker>();
    public List<NodeTracker> connectionLines = new List<NodeTracker>();

    Vector3 GoToPosition;
    Vector3 velocity;

    public bool UpdatePosition { get; private set; }

    public float updateSpeed = 1000f;

    // TODO: Use spherical Transform to do some cool stuff!
    public void SetNewPosiiton(Vector3 newPos)
    {
        UpdatePosition = true;
        GoToPosition = newPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdatePosition)
        {
            //transform.position = Vector3.SmoothDamp(transform.position, GoToPosition, ref velocity, updateSpeed * Time.deltaTime );
            transform.position = Vector3.Slerp(transform.position, GoToPosition, 10 * Time.deltaTime);
        }
    }
}
