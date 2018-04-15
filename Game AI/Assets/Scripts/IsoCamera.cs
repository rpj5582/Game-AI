using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour
{
    public int translationBorder = 100;

    public float moveSpeed = 25.0f;

    public float zoomMinY = 5.0f;
    public float zoomMaxY = 20.0f;

    void Start()
    {

    }

    void Update()
    {
        // Translational movement
        Vector3 localRight = transform.right;
        Vector3 localForward = /*Vector3.ProjectOnPlane(transform.forward, Vector3.up);*/transform.up;
        localForward.Normalize();

        float h = 0;
        float v = 0;
        Vector2 mousePosition = Input.mousePosition;
        if (mousePosition.x > 0 && mousePosition.x < translationBorder)
            h = -1;
        if (mousePosition.x < Screen.width && mousePosition.x > Screen.width - translationBorder)
            h = 1;

        if (mousePosition.y > 0 && mousePosition.y < translationBorder)
            v = -1;
        if (mousePosition.y < Screen.height && mousePosition.y > Screen.height - translationBorder)
            v = 1;

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
