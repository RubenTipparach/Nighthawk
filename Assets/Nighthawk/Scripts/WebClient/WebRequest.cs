using Assets.Nighthawk.Scripts.Searchsploit_Tools;
using Newtonsoft.Json;
using nmap_tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    [SerializeField]
    bool testConnection = false;

    [SerializeField]
    bool makeScan = false;


    [SerializeField]
    bool testScanTarget = false;

    [SerializeField]
    bool isTestVal = false;


    [SerializeField]
    string initialUrl = "";

    public bool ServerIsOnline { get; private set; }

    [SerializeField]
    ScanHostsPayload scanableTargets;

    //[SerializeField]
    public ScanTargetPayload scanTargetPayload { get; private set; }

    public NmapRun NmapRun { get; private set; }

    public string[] listEnabledExploit;

    [SerializeField]
    SploitResults[] _scannedExploits;

    public SploitResults[] ScannedExploits
    {
        get { return _scannedExploits; }
    }

    private void Awake()
    {
        ServerIsOnline = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        TestConnection();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle connection test
        FlipBool(ref testConnection, TestConnection);

        // toggle scan server
        FlipBool(ref makeScan, ScanServer);

        // Test the scan target function.
        FlipBool(ref testScanTarget, ()=> ScanTarget("134.129.92.10"));

    }

    private void FlipBool(ref bool testVal, Action urlCallback){
        if (testVal)
        {
            urlCallback();
            testVal = false;
        }
    }

    public void TestConnection()
    {
        StartCoroutine(GetRequest(initialUrl,
            (result) => {
                Debug.Log("server is online!");
                ServerIsOnline = true;
            }));
    }


    public void ScanServer(){
        if (!ServerIsOnline)
        {
            Debug.Log("Server is offline, can't do anything.");
            return;
        }

        StartCoroutine(GetRequest(initialUrl + "/scan",
        (result) => {
            Debug.Log("********************** SCAN **********************");
            Debug.Log(result);

            scanableTargets = JsonUtility.FromJson<ScanHostsPayload>(result);
        }));
    }

    public void ScanTarget(string ipAdress){
        if (!ServerIsOnline)
        {
            Debug.Log("Server is offline, can't do anything.");
            return;
        }

        StartCoroutine(GetRequest(initialUrl + "/scantarget/"+ ipAdress.Replace('.','_') + "/"+ (isTestVal ? 1: 0),
        (result) => {
            Debug.Log("********************** SCANTARGET **********************");
            Debug.Log(result);

            scanTargetPayload = JsonConvert.DeserializeObject<ScanTargetPayload>(result);
            NmapRun = ReadNmapXML(scanTargetPayload.scan_target_data);

            listEnabledExploit = scanTargetPayload.scan_target_exploits.exploit_hardcoded;

            _scannedExploits = scanTargetPayload.scan_target_exploits.scannedExploits;

        }));
    }

    public NmapRun ReadNmapXML(string text)
    {
        XmlSerializer s = new XmlSerializer(typeof(NmapRun));
        using (TextReader reader = new StringReader(text))
        {
            return (NmapRun)s.Deserialize(reader);
        }
    }

    [Serializable]
    public class ScanHostsPayload {
        public string[] data;
    }

    [Serializable]
    public class ScanTargetPayload {

        public string message;

        public string scan_target_data;

        public SearchSploitScans scan_target_exploits;
    }

    [Serializable]
    public class SearchSploitScans
    {
        public string[] exploit_hardcoded;

        public SploitResults[] scannedExploits;
    }

    IEnumerator GetRequest(string uri, Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                string result = webRequest.downloadHandler.text;
                callback(result);
            }
        }
    }
}
