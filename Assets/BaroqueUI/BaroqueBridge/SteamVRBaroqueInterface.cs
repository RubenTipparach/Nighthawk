using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.Linq;
using Valve.VR;

public class SteamVRBaroqueInterface : MonoBehaviour
{
    public static SteamVRBaroqueInterface svrBaroqueInterface;

    public SteamVR_Behaviour_Pose steamVR_ControllerManager_left;

    public SteamVR_Behaviour_Pose steamVR_ControllerManager_right;

    private void Awake()
    {
        //if (steamVR_ControllerManager == null) 
        //    steamVR_ControllerManager = GetComponent<SDK_SteamVRInputSource>();

        SteamVR_Behaviour_Pose[] vrsources = FindObjectsOfType<SteamVR_Behaviour_Pose>();

        if (steamVR_ControllerManager_left == null)
        {
            steamVR_ControllerManager_left = vrsources.FirstOrDefault(p => p.inputSource == Valve.VR.SteamVR_Input_Sources.LeftHand);
        }

        if(steamVR_ControllerManager_right == null)
        {
            steamVR_ControllerManager_right = vrsources.FirstOrDefault(p => p.inputSource == Valve.VR.SteamVR_Input_Sources.RightHand);
        }

        svrBaroqueInterface = this;
    }

}
