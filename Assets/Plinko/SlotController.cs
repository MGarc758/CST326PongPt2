using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{

    public int slotNumber;
    public int pointValue;

    private void OnTriggerEnter(Collider other) {
        Debug.Log($"entered {slotNumber} worth {pointValue} points");
    }
}
