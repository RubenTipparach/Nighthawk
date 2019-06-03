using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class EditorNodeConnector : MonoBehaviour
{
    [SerializeField]
    Transform source;

    [SerializeField]
    Transform destination;

    LineRenderer line;

    [SerializeField]
    float width = 0.5f;

    [SerializeField]
    float widthEnd = 0.5f;

    [SerializeField]
    bool useWidthEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (source != null && destination != null)
        {
            UpdateWidth();
            line.SetPosition(0, source.position);
            line.SetPosition(1, destination.position);
        }

        if(source == null && destination != null)
        {
            UpdateWidth();
            line.SetPosition(0, transform.position);
            line.SetPosition(1, destination.position);
        }
    }

    private void UpdateWidth()
    {
        if (useWidthEnd)
        {
            line.widthCurve = new AnimationCurve(new Keyframe(0, width), new Keyframe(0, widthEnd));
        }
        else
        {
            line.widthCurve = new AnimationCurve(new Keyframe(0, width));
        }
    }
}
