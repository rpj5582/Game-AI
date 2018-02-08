using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour
{
    public float moveSpeed = 25.0f;
    public float zoomMinY = 5.0f;
    public float zoomMaxY = 20.0f;

    void Start()
    {

    }

    void LateUpdate()
    {
        // Translational movement
        Vector3 localRight = transform.right;
        Vector3 localForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        localForward.Normalize();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += localRight * h * moveSpeed * Time.deltaTime;
        transform.position += localForward * v * moveSpeed * Time.deltaTime;

        // Zoom
        float scrollDelta = Input.mouseScrollDelta.y;
        if(scrollDelta < 0)
        {
            if (transform.position.y + scrollDelta < zoomMaxY)
            {
                transform.position += transform.forward * scrollDelta;
            }
        }
        else
        {
            if (transform.position.y + scrollDelta > zoomMinY)
            {
                transform.position += transform.forward * scrollDelta;
            }
        }
    }
}
