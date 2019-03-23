using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaroqueUI;

public class MenuControl : MonoBehaviour
{

    public KeyboardClicker numpadIP;

    // Start is called before the first frame update
    void Start()
    {
        var ct = Controller.GlobalTracker(this);
        ct.onTriggerDown += OnTriggerDown;
    }

    // Update is called once per frame
    void OnTriggerDown(Controller controller)
    {
        //Dialog d;
        //d.MakePopup(null, null);

        // determine direction
        Vector3 head_forward = controller.position - Baroque.GetHeadTransform().position;
        Vector3 fw = controller.forward + head_forward.normalized;
        fw.y = 0; 
        Vector3 handposition = controller.position + (0.05f * fw);

        // spawn numpad
        var popup = Instantiate( numpadIP, handposition, Quaternion.LookRotation(fw));//.MakePopup(controller, gameObject);
        if (popup == null)
            return;

    }
}

