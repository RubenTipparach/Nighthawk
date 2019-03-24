using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaroqueUI;

public class MenuControl : MonoBehaviour
{

    public GameObject searchInterface;
    public KeyboardClicker numpadIP;

    private KeyboardClicker popup;




    // Start is called before the first frame update
    void Start()
    {
        // set up trigger
        var ct = Controller.GlobalTracker(this);
        ct.onTriggerDown += OnTriggerDown;

        // spawn keypad
        popup = Instantiate(numpadIP, this.transform.position, this.transform.rotation);

        // disable and turn invisible
        popup.enabled = false;
        popup.GetComponent<Canvas>().enabled = false;

        // connect to searchInterface
        popup.GetComponentInChildren<SearchIPs>().searchInterface = searchInterface;
    }

    // Update is called once per frame
    void OnTriggerDown(Controller controller)
    {
        if (!popup.enabled)
        {
            // determine direction
            Vector3 head_forward = controller.position - Baroque.GetHeadTransform().position;
            Vector3 fw = controller.forward + head_forward.normalized;
            fw.y = 0;
            Vector3 handposition = controller.position + (0.05f * fw);

            // move numpad
            popup.transform.position = handposition;
            popup.transform.rotation = Quaternion.LookRotation(fw);
        }

        // flip enable
        popup.enabled = !popup.enabled;
        
        // flip visibility
        var popupRenderer = popup.GetComponent<Canvas>();
        popupRenderer.enabled = !popupRenderer.enabled;

    }
}

