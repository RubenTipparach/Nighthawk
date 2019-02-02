using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SteamVR_ControllerManager))]
public class SteamVRBaroqueInterface : MonoBehaviour
{
    public static SteamVRBaroqueInterface svrBaroqueInterface;

    public SteamVR_ControllerManager steamVR_ControllerManager { get; private set; }

    private void Awake()
    {
        steamVR_ControllerManager = GetComponent<SteamVR_ControllerManager>();
        svrBaroqueInterface = this;
    }

}
