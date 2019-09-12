using Hover.Core.Items.Types;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class HoverItemWriteSearch : MonoBehaviour
{
    public SpatialSearchInterface searchScript;

    public HoverItemDataText origin;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void Append(string v)
    {
        NonRegexCheck(v + origin.Label );
    }

    public void Delete()
    {
        NonRegexCheck("");
    }

    // Update is called once per frame
    public void RegexCheck(string input)
    {
        if (Regex.IsMatch(input, "^(?:[0-9]{1,3}\\.){3}[0-9]{1,3}$"))
        {
            Debug.Log("match: " + input);
        }
        else
        {
            Debug.Log("not a match: " + input);
        }
    }

    public void NonRegexCheck(string input)
    {
        //if(input)
        string[] splitInput = input.Split('.');
        int len = splitInput.Length;

        Debug.Log(input + "   (" + len + ")");

        // check for too many decimals
        if (len > 4)
        {
            Debug.Log("detected error!");
            input = input.Substring(0, (input.Length - 1));
        }

        // re-evaluate splitInput
        splitInput = input.Split('.');
        len = splitInput.Length;

        // check for out of range values
        for (int i = 0; i < len; i++)
        {
            string substr = splitInput[i];
            int iSubStr;
            if (int.TryParse(substr, out iSubStr) && iSubStr > 255)
            {
                substr = "255";
                splitInput[i] = substr;
            }

        }

        // concatenate and push
        string output = "";
        for (int i = 0; i < len; i++)
        {
            output += splitInput[i];
            if (i != 3)
            {
                output += ".";
            }
        }

        origin.Label = output;

        //Debug.Log("output: " + origin.text);
    }

    public void ConfirmSearch()
    {
        string ip = origin.Label;
        startSearch(ip);
    }

    private void startSearch(string ip)
    {
        
        string[] splitIP = ip.Split('.');
        int[] results = new int[4];
        for (int i = 0; i < splitIP.Length; i++)
        {
            if (splitIP[i] == "")
                results[i] = -1;
            else
                results[i] = int.Parse(splitIP[i]);
        }

        searchScript.oc1 = results[0];
        searchScript.oc2 = results[1];
        searchScript.oc3 = results[2];
        searchScript.oc4 = results[3];

        searchScript.triggerUpdate = true;
    }
}
