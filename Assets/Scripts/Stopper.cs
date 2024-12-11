using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Stopper : MonoBehaviour
{
    public bool isStopperUse = true; // trueの場合に処理を実行する

    public float rayDistance = 8f;

    void Update()
    {
        if (!isStopperUse)
            return;

        if (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Kagu"))
                {
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    StopperObjectActive stopperObjectActive = hit.collider.GetComponent<StopperObjectActive>();

                    if (rb != null)
                    {
                        stopperObjectActive.isStopperActive = true;
                        stopperObjectActive.StopperActive();
                        Destroy(rb);
                        Debug.Log("Rigidbody destroyed for object with 'kagu' tag.");
                    }
                }
            }
        }
    }

    public bool GetIsStopperUse()
    {
        return isStopperUse;
    }

    public void SetIsStopperUse(bool value)
    {
        isStopperUse = value;
    }

}
