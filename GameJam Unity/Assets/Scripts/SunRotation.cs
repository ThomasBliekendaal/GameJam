using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAroundLocal(gameObject.transform.right, rotationSpeed);
    }
}
