using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public float range;
    public float speed;
    public SphereCollider trigger;

    private void Start()
    {
        Debug.Log("boom");
        trigger.radius = 0;
    }

    private void Update()
    {
        trigger.radius += speed;
        Debug.Log("grow");
        if(trigger.radius >= range)
        {
            Debug.Log("done");
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("found thing");
        if(other.tag == "Enemy")
        {
            Debug.Log("do damage");
            other.GetComponent<EnemyUnit>().LoseHP(damage);
        }
    }
}
