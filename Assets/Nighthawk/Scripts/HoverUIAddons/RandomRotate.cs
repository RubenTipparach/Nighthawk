using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    float minSpeed = 1;

    float maxSpeed = 30;

    float RotationSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        RotationSpeed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
