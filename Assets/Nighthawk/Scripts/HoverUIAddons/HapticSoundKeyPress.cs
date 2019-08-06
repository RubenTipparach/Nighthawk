using Hover.Core.Items.Types;
using Hover.InterfaceModules.Key;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HapticSoundKeyPress : MonoBehaviour
{
    public SteamVR_Action_Vibration haptic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeHapticFeedbback(IItemDataSelectable itemdataSelectable, HoverkeyItemLabels labels)
    {
        //Debug.Log($"item select label: {itemdataSelectable.Label}");
        Debug.Log($"label data: {labels.DefaultLabel}");

        //todo determine hand.
        haptic.Execute(0, .05f, 75, 25, SteamVR_Input_Sources.RightHand);
           
    }


}
