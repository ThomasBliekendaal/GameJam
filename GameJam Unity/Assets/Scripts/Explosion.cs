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
        trigger.radius = 0;
    }

    private void Update()
    {
        trigger.radius += speed;
        if(trigger.radius >= range)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyUnit>().LoseHP(damage);
        }
    }
}
