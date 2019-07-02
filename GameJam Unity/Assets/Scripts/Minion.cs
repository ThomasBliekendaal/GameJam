using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public int damage;
    public float fireRate;
    private float fireTimer;
    public float hp;
    public GameObject target;
    public float searchGrowth;
    public SphereCollider trigger;
    private NavMeshAgent agent;
    public List<EnemyUnit> targetedBy = new List<EnemyUnit>();

    private void Start()
    {
        fireTimer = fireRate;
        agent = GetComponent<NavMeshAgent>();
        ResetTarget();
    }

    private void Update()
    {
        if(target == null)
        {
            Search();
        }
        else
        {
            agent.destination = target.transform.position;
            if(fireTimer <= 0)
            {
                target.GetComponent<EnemyUnit>().LoseHP(damage);
                fireTimer = fireRate;
            }
        }
        if(hp <= 0)
        {
            Death();
        }
    }

    public void ResetTarget()
    {
        trigger.radius = 0;
        target = null;
    }

    public void Search()
    {
        trigger.radius += searchGrowth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            target = other.gameObject;
            target.GetComponent<EnemyUnit>().targetedByMinions.Add(gameObject.GetComponent<Minion>());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        fireTimer -= Time.deltaTime;
    }

    private void Death()
    {
        foreach(EnemyUnit e in targetedBy)
        {
            e.ClearTarget();
        }
    }

}
