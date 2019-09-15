using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.IO;
using VRTK;
using Hover.Core.Cursors;

public class AccuracyCallibrator : MonoBehaviour
{
    public SteamVR_Action_Boolean button1Press;

    public SteamVR_Action_Boolean button2Press;

    public SteamVR_Action_Boolean buttonTrigger;

    public SteamVR_Input_Sources leftOrRightHand;

    [SerializeField]
    VRTK_UIPointer uiPointer;

    [SerializeField]
    Transform HandSourcePoint;

    [SerializeField]
    HoverCursorData hoverData;

    [SerializeField]
    VR_Framework frameworkType;

    bool recording = false;

    Vector3 origin;
    Vector3 destination;

    float time;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (button1Press.GetStateDown(leftOrRightHand))
        {
            if (!recording)
            {
                BeginRecording();
            }
            else
            {
                EndRecording();
            }
        }

        if (buttonTrigger.GetStateDown(leftOrRightHand))
        {
            DetectClick();
        }

        if (recording)
        {
            time = Time.unscaledTime;

            // data collection method.
            if (frameworkType == VR_Framework.VRTK)
            {
                origin = uiPointer.GetOriginPosition();
                if (uiPointer.pointerEventData != null && uiPointer.pointerEventData.pointerCurrentRaycast.isValid)
                {
                    destination = uiPointer.pointerEventData.pointerCurrentRaycast.worldPosition;
                    Debug.Log($"origin: {origin.ToString("G")} destination: {destination.ToString("G")}");
                    FormatInputData();
                }
            }

            if (frameworkType == VR_Framework.HoverUI)
            {
                
                origin = HandSourcePoint.position;

                if (hoverData.BestRaycastResult.HasValue)
                {
                    Debug.LogWarning("Best Raycast has value!");
                    //hoverData.BestRaycastResult.Value.WorldPosition
                }

                destination = HandSourcePoint.forward * 0.05f + HandSourcePoint.position;
                //destination = HandSourcePoint.forward * (0.05f) + origin;//simulate small transform
                Debug.Log($"origin: {origin.ToString("G")} destination: {destination.ToString("G")}");
                FormatInputData();
                //Debug.Log(hoverData.Idle.Progress);
                //if (hoverData.Idle.IsActive)
                //{

                //}
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (recording)
        {
            // origin = uiPointer.GetOriginPosition();
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, destination);
        }
    }

    public void BeginRecording()
    {
        recording = true;
    }

    public void EndRecording()
    {
        recording = false;
    }

    public void FormatInputData()
    {
        StartCoroutine(WriteString($"{time},{origin.x},{origin.y},{origin.z},{destination.x},{destination.y},{destination.z},INPUT"));
    }

    public void DetectClick()
    {
        Debug.LogWarning("TRIGGER PRESSED");

        StartCoroutine(WriteString($"{time},{origin.x},{origin.y},{origin.z},{destination.x},{destination.y},{destination.z},TRIGGER"));
    }

    [SerializeField]
    string assetPath = "Assets/Resources/data_collection_a1.txt";

    private IEnumerator WriteString(string write)
    {

        string path = assetPath;

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(write);
        writer.Close();

        yield return null;

        //Re-import the file to update the reference in the editor

        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load("test");


        ////Print the text from the file
        //Debug.Log(asset.text);
    }
    public enum PlayerType
    {
        STEAMVR_1X,
        STEAMVR_2X
    }

    public enum VR_Framework
    {
        Baroque,
        VRTK,
        HoverUI     
    }
}

