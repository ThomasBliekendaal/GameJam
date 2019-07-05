using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public int damage;
    public float fireRate;
    [SerializeField] private float fireTimer;
    public float hp;
    public GameObject target;
    public float searchGrowth;
    public SphereCollider trigger;
    private NavMeshAgent agent;
    public List<EnemyUnit> targetedBy = new List<EnemyUnit>();
    private Animator anim;
    private GameManager manager;
    private IEnumerator coroutine;

    private void Start()
    {
        coroutine = Dying(5);
        fireTimer = fireRate;
        agent = GetComponent<NavMeshAgent>();
        ResetTarget();
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(target == null)
        {
            Search();
        }
        else
        {
            SetAnim("Walking");
            agent.destination = target.transform.position;
            if(fireTimer <= 0)
            {
                SetAnim("Attacking");
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
        target = manager.enemyUnits[Random.Range(0, manager.enemyUnits.Count)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            target = other.gameObject;
            target.GetComponent<EnemyUnit>().targetedByMinions.Add(gameObject.GetComponent<Minion>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            fireTimer -= Time.deltaTime;
        }
    }

    private void Death()
    {
        SetAnim("Dying");
        foreach(EnemyUnit e in targetedBy)
        {
            e.ClearTarget();
        }
        StartCoroutine(coroutine);
    }

    private void SetAnim(string animation)
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDying", false);
        if(animation == "Walking")
        {
            anim.SetBool("isWalking", true);
        }
        if(animation == "Attacking")
        {
            anim.SetBool("isAttacking", true);
        }
        if(animation == "Dying")
        {
            anim.SetBool("isDying", true);
        }
    }

    private IEnumerator Dying(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
