using nmap_tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class NMapRead : MonoBehaviour
{
    [SerializeField]
    bool nmapRead = false;

    public TextAsset txt;

    public NmapRun NmapRunData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReadNmapXML(){
        XmlSerializer s = new XmlSerializer(typeof(NmapRun));
        using (TextReader reader = new StringReader(txt.text))
        {
            NmapRunData = (NmapRun)s.Deserialize(reader);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(nmapRead)
        {
            ReadNmapXML();
            nmapRead = false;
        }
    }
}
