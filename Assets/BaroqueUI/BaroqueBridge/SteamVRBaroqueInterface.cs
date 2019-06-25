using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SteamVRBaroqueInterface : MonoBehaviour
{
    public static SteamVRBaroqueInterface svrBaroqueInterface;

    public SteamVR_ControllerManager steamVR_ControllerManager;
    private void Awake()
    {
        if (steamVR_ControllerManager == null) 
            steamVR_ControllerManager = GetComponent<SteamVR_ControllerManager>();
        svrBaroqueInterface = this;
    }

}
