using Mapbox.Unity.Map;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed;

    [SerializeField]
    float moveSpeed;

    float smoothZoom;

    float smoothSpeed;

    [SerializeField]
    bool debugMode = true;

    AbstractMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<AbstractMap>();
    }

    // Update is called once per frame
    void Update()
    {
        double x = 0;
        double y = 0;

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");


        if(Input.GetKey(KeyCode.R))
        {
            map.SetZoom(map.Zoom + 1 * zoomSpeed);
            Debug.Log(map.Zoom);
        }

        Debug.Log("WTF!!!!");

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("WTF!!!! 2");

            Debug.Log(map.Zoom);
            map.SetZoom(map.Zoom - 1*zoomSpeed);
        }



        //if (x != 0 || y != 0)
        //{
        //    var c = map.CenterLatitudeLongitude;
        //    Vector2d location = (new Vector2d(x, y ) + c) * moveSpeed * Time.deltaTime;
        //    map.SetCenterLatitudeLongitude(location);
        //}
    }
}
