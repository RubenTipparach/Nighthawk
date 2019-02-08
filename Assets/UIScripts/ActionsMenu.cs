using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaroqueUI;

public class ActionsMenu : MonoBehaviour
{

    public Dialog dialogChooseAction;

    // Start is called before the first frame update
    void Start()
    {
        var ct = Controller.HoverTracker(this);
        ct.onTriggerDown += OnTriggerDown;
    }

    // Update is called once per frame
    void OnTriggerDown(Controller controller)
    {
        var popup = dialogChooseAction.MakePopup(controller, gameObject);
        if (popup == null)
            return;

        popup.SetChoices("DynDropdown", new List<string> {
            "Option A", "Option B", "Option C"
        });


    }
}
