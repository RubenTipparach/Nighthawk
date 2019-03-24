using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class numkeyRegex : MonoBehaviour
{

    public InputField origin;

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

        Debug.Log(input +  "   (" + len+")");

        // check for too many decimals
        if ( len > 4 )
        {
            Debug.Log("detected error!");
            input = input.Substring(0, (input.Length - 1));
        }

        // re-evaluate splitInput
        splitInput = input.Split('.');
        len = splitInput.Length;

        // check for out of range values
        for ( int i = 0; i < len; i++ )
        {
            string substr = splitInput[i];
            int iSubStr;
            if ( int.TryParse(substr, out iSubStr) && iSubStr > 255 )
            {
                substr = "255";
                splitInput[i] = substr;
            }
            
        }

        // concatenate and push
        string output = "";
        for ( int i = 0; i < len; i++ )
        {
            output += splitInput[i];
            if ( i != 3)
            {
                output += ".";
            }
        }
        origin.text = output;

        Debug.Log("output: " + origin.text);
    }
}
