using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float maxPosRight;
    public float maxPosLeft;
    public float trueZoom;
    public float trueZoomMax = 1F;
    public float trueZoomMin = 0F;
    public float zoomAmount;
    public float moveSpeed = 3;
    public float moveSpeedFromStart = 3;
    public float moveSpeedToAdd = 6;
    public float rotateSpeed = 1;
    public float hor;
    public float camHor;
    public Vector3 camPos;
    public Vector3 camRot;

    public GameObject CameraZoomPosition1;
    public GameObject CameraZoomPosition2;

    private void Start()
    {
        GetClampValues();
    }

    public void Update()
    {
        CamMovement();
    }

    public void GetClampValues()
    {
        camPos.x = -11.06592F;
        maxPosRight = camPos.x + 5.5F;
        maxPosLeft = camPos.x - 5.5F;
    }

    public void CamMovement()
    {
        //movement button
        camPos.x = Input.GetAxis("Horizontal");

        //set movement to transform and equalize the speed
        gameObject.transform.Translate(camPos * Time.deltaTime * moveSpeed);
        CameraZoomPosition1.transform.Translate(camPos * Time.deltaTime * moveSpeed);
        CameraZoomPosition2.transform.Translate(camPos * Time.deltaTime * moveSpeed);

        //Zoom
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomAmount = Input.GetAxis("Mouse ScrollWheel");
            trueZoom = trueZoom + zoomAmount;
            CamZoom();
        }

        //speed up camera move speed
        if (Input.GetButtonDown("SpeedCamera"))
        {
            moveSpeed = moveSpeed + moveSpeedToAdd;
        }
        if (Input.GetButtonUp("SpeedCamera"))
        {
            moveSpeed = moveSpeedFromStart;
        }

        //clamp the true zoom amount
        if(trueZoom >= trueZoomMax)
        {
            trueZoom = trueZoomMax;
        }

        if (trueZoom <= trueZoomMin)
        {
            trueZoom = trueZoomMin;
        }

        //clamp the camera positions
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, maxPosLeft, maxPosRight), transform.position.y, transform.position.z);
    }

    public void CamZoom()
    {
        transform.position = Vector3.Lerp(CameraZoomPosition1.GetComponent<Transform>().position, CameraZoomPosition2.GetComponent<Transform>().position, trueZoom);
    }
}
