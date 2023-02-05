using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddle : MonoBehaviour
{
    public float unitsPerSecond = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per framerate
    void FixedUpdate()
    {
        float horizontalValue = Input.GetAxis("RightPaddle");
        Vector3 force = Vector3.right * horizontalValue;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }
}
