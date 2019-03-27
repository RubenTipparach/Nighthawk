using Assets.Nighthawk.Scripts.Searchsploit_Tools;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchsploitRead : MonoBehaviour
{
    [SerializeField]
    TextAsset txt;

    [SerializeField]
    bool runRead = false;

    SearchsploitJson searchsploitData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(runRead)
        {
            ReadSploits();
            runRead = false;
        }
    }

    public void ReadSploits()
    {
        searchsploitData = JsonConvert.DeserializeObject<SearchsploitJson>(txt.text);
    }
}
