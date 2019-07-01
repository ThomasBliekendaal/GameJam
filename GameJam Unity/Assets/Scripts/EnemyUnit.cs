using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
    public AIUnit type;
    public SphereCollider attackTrigger;
    public List<PlayerUnit> targetedBy = new List<PlayerUnit>();
    private GameManager gameManager;
    private NavMeshAgent agent;
    private GameObject target;

    private int hp;
    private int damage;
    private float attackRange;
    private float detectRange;
    private float fireRate;
    private float fireTimer;
    private bool attacking;

    private void Awake()
    {
        hp = type.hp;
        damage = type.damage;
        attackRange = type.attackRange;
        detectRange = type.detectRange;
        fireRate = type.fireRate;
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TargetSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            TargetSelect();
        }
        else
        {
            agent.destination = target.transform.position;
        }
        if(attacking == true)
        {
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0)
            {
                Attack();
                fireTimer = fireRate;
            }
        }
    }

    private void Attack()
    {
        target.GetComponent<PlayerUnit>().LoseHP(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target.gameObject)
        {
            attacking = true;
        }
    }

    public void LoseHP(int i)
    {
        hp -= i;
        if(hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        foreach(PlayerUnit p in targetedBy)
        {
            p.ClearTarget();
            p.targetedBy.Remove(gameObject.GetComponent<EnemyUnit>());
        }
        Destroy(gameObject);
    }

    public void ClearTarget()
    {
        target = null;
    }

    private void TargetSelect()
    {
        if (gameManager.meleeUnits.Count > 0)
        {
            target = gameManager.meleeUnits[Random.Range(0, gameManager.meleeUnits.Count)];
            target.GetComponent<PlayerUnit>().targetedBy.Add(gameObject.GetComponent<EnemyUnit>());
        }
        else if (gameManager.rangedUnits.Count > 0)
        {
            target = gameManager.rangedUnits[Random.Range(0, gameManager.rangedUnits.Count)];
            target.GetComponent<PlayerUnit>().targetedBy.Add(gameObject.GetComponent<EnemyUnit>());
        }
        else if (gameManager.supportUnits.Count > 0)
        {
            target = gameManager.supportUnits[Random.Range(0, gameManager.supportUnits.Count)];
            target.GetComponent<PlayerUnit>().targetedBy.Add(gameObject.GetComponent<EnemyUnit>());
        }
        else
        {
            Debug.Log("No targets");
        }
        if(target != null)
        {
            target.GetComponent<PlayerUnit>().targetedBy.Add(gameObject.GetComponent<EnemyUnit>());
        }
    }
}
