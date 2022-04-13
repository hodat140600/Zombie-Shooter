using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRay : MonoBehaviour
{
    RaycastHit hit;
    public Transform rayShootPoint;
    float rayDistance = 3f;
    public LayerMask layer;
    float tick = 1f;

    private void Update()
    {
        tick -= Time.deltaTime;
        if (tick <= 0)
        {
            tick = 1f;
            Debug.DrawRay(rayShootPoint.position, rayShootPoint.forward, Color.red);
            if (Physics.Raycast(rayShootPoint.position, rayShootPoint.forward, out hit, rayDistance, layer)){ 
                DoorTrigger doorTriggerScript = hit.transform.GetComponent<DoorTrigger>();
                if (!doorTriggerScript.isOpen && !doorTriggerScript.isLooked)
                {
                    doorTriggerScript.OpenDoor();
                }
            }
        }
    }
}
