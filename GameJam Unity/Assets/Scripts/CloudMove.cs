using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float maxHeight = 0.002F;
    public float minHeight = -0.002F;
    private float posToMoveTo;
    public float timeUntilMove = 10F;
    public float timePassed;

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed = timePassed + Time.deltaTime;
        SetNewPos();
        transform.Translate(0, posToMoveTo, 0);
    }

    void SetNewPos()
    {
        if (timePassed > timeUntilMove)
        {
            posToMoveTo = Random.Range(minHeight, maxHeight);
            print("posToMoveTo = " + posToMoveTo);
            timePassed = 0F;
        }
    }
}
