using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float maxPosRight;
    public float maxPosLeft;
    public float zoomAmount;
    public float moveSpeed = 1;
    public float rotateSpeed = 1;
    public float hor;
    public float camHor;
    public Vector3 camPos;
    public Vector3 camRot;

    public GameObject CameraZoomPosition1;
    public GameObject CameraZoomPosition2;

    private void Start()
    {
        //GetClampValues();
    }

    public void Update()
    {
        CamMovement();
        CamZoom();
    }

    public void GetClampValues()
    {
        camPos = gameObject.GetComponent<Transform>().position;
        maxPosRight = camPos.x + 20.5F;
        maxPosLeft = camPos.x - 20.5F;
    }

    public void CamMovement()
    {
        //movement button
        hor = Input.GetAxis("Horizontal");
        zoomAmount = Input.GetAxis("Mouse ScrollWheel");

        //set axis
        camPos.x = hor;

        //set movement to transform and equalize the speed
        transform.Translate(camPos * Time.deltaTime * moveSpeed);

        //clamp the camera positions
        //if(camPos.x > maxPosLeft)
        //{
        //    camPos.x = maxPosLeft;
        //}

        //if (camPos.x > maxPosRight)
        //{
        //   camPos.x = maxPosRight;
        //}
    }

    public void CamZoom()
    {
        transform.position = Vector3.Lerp(CameraZoomPosition1.GetComponent<Transform>().position, CameraZoomPosition2.GetComponent<Transform>().position, zoomAmount);
    }
}
