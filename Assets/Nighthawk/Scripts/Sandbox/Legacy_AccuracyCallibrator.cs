using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Legacy_AccuracyCallibrator : MonoBehaviour
{
    public SteamVR_TrackedController controller;

    //public SteamVR_Action_Boolean button1Press;

    //public SteamVR_Action_Boolean button2Press;

    //public SteamVR_Action_Boolean buttonTrigger;

    //public SteamVR_Input_Sources leftOrRightHand;


    [SerializeField]
    public Transform HandSourcePoint;

    [SerializeField]
    VR_Framework frameworkType;

    bool recording = false;

    Vector3 origin;
    Vector3 destination;

    float time;

    bool button1Press = false;
    bool buttonTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        controller.MenuButtonClicked += Controller_MenuButtonClicked;
        controller.TriggerClicked += Controller_TriggerClicked;
    }

    private void Controller_TriggerClicked(object sender, ClickedEventArgs e)
    {
        DetectClick();
    }

    private void Controller_MenuButtonClicked(object sender, ClickedEventArgs e)
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

    // Update is called once per frame
    void Update()
    {

        if (recording)
        {
            time = Time.unscaledTime;

            // data collection method.
            //if (frameworkType == VR_Framework.VRTK)
            //{
            //    origin = uiPointer.GetOriginPosition();
            //    if (uiPointer.pointerEventData != null && uiPointer.pointerEventData.pointerCurrentRaycast.isValid)
            //    {
            //        destination = uiPointer.pointerEventData.pointerCurrentRaycast.worldPosition;
            //        Debug.Log($"origin: {origin.ToString("G")} destination: {destination.ToString("G")}");
            //        FormatInputData();
            //    }
            //}

            //if (frameworkType == VR_Framework.HoverUI)
            //{

            //    origin = HandSourcePoint.position;

            //    if (hoverData.BestRaycastResult.HasValue)
            //    {
            //        Debug.LogWarning("Best Raycast has value!");
            //        //hoverData.BestRaycastResult.Value.WorldPosition
            //    }

            //    destination = HandSourcePoint.forward * 0.05f + HandSourcePoint.position;
            //    //destination = HandSourcePoint.forward * (0.05f) + origin;//simulate small transform
            //    Debug.Log($"origin: {origin.ToString("G")} destination: {destination.ToString("G")}");
            //    FormatInputData();
            //    //Debug.Log(hoverData.Idle.Progress);
            //    //if (hoverData.Idle.IsActive)
            //    //{

            //    //}
            //}

            if (frameworkType == VR_Framework.Baroque)
            {
                origin = HandSourcePoint.position;
                destination = HandSourcePoint.position;
                Debug.Log($"origin: {origin.ToString("G")} destination: {destination.ToString("G")}");
                FormatInputData();
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
