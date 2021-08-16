using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionObjetos : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float velocity = 1f;
    public float returnSpeed = 0.5f;

    //private Quaternion originalRotation;

    void Start()
    {
        //originalRotation = gameObject.transform.rotation;
    }

    private void Update()
    {
        //Quaternion targetRotation;

        if (Input.GetMouseButton(0))
        {
            OnMouseDrag();
        }

        //Return to original transform
        /*else
        {
            Vector3 currentRotation = gameObject.transform.rotation.eulerAngles;
            float step = velocity * Time.deltaTime;
            targetRotation = Quaternion.Euler(0, currentRotation.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);
        }*/
    }

    void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

        transform.RotateAroundLocal(Vector3.up, -XaxisRotation);
        transform.RotateAroundLocal(Vector3.right, YaxisRotation);
    }
}
