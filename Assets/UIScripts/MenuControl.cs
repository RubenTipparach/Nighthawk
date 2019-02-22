using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaroqueUI;

public class MenuControl : MonoBehaviour
{

    public Dialog numpadIP;

    // Start is called before the first frame update
    void Start()
    {
        var ct = Controller.GlobalTracker(this);
        ct.onTriggerDown += OnTriggerDown;
    }

    // Update is called once per frame
    void OnTriggerDown(Controller controller)
    {
        var popup = numpadIP.MakePopup(controller, gameObject);
        if (popup == null)
            return;

    }
}

