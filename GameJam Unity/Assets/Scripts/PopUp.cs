using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1 * speed, 0));

        lifeTime -= Time.deltaTime;
        if(lifeTime<= 0)
        {
            Destroy(gameObject);
        }
    }
}
