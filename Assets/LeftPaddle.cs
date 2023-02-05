using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : MonoBehaviour
{
    public float unitsPerSecond = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per framerate
    void FixedUpdate()
    {
        float horizontalValue = Input.GetAxis("LeftPaddle");
        Vector3 force = Vector3.right * horizontalValue;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
