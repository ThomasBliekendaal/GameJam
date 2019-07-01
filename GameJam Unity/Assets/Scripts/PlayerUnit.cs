using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnit : MonoBehaviour
{
    public Unit type;
    public SphereCollider rangeTrigger;
    private GameObject target;
    public GameObject endGoal;
    private NavMeshAgent agent;
    private GameManager gameManager;
    private float fireTimer;
    public List<EnemyUnit> targetedBy = new List<EnemyUnit>();

    private int hp;
    private int damage;
    private float range;
    private float fireRate;
    private float abilityCooldownOne;
    private float abilityCooldownTwo;
    private float cooldownTimer;

    // Start is called before the first frame update

    private void Awake()
    {
        hp = type.hp;
        damage = type.damage;
        range = type.range;
        fireRate = type.fireRate;
        abilityCooldownOne = type.abilityCooldownOne;
        abilityCooldownTwo = type.abilityCooldownTwo;
        cooldownTimer = 0;
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        rangeTrigger.radius = range;
        endGoal = gameManager.endGoals[Random.Range(0, gameManager.endGoals.Count)];
        agent.destination = endGoal.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            agent.destination = gameObject.transform.position;
            transform.LookAt(target.transform.position);
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0)
            {
                Attack();
                fireTimer = fireRate;
            }
        }
        else
        {
            agent.destination = endGoal.transform.position;
        }

        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target == null && other.tag != "Terrain")
        {
            target = other.gameObject;
            other.gameObject.GetComponent<EnemyUnit>().targetedBy.Add(this.gameObject.GetComponent<PlayerUnit>());
        }
    }

    public void Attack()
    {
        target.GetComponent<EnemyUnit>().LoseHP(damage);
    }

    public virtual void Ability(int ability)
    {
        if (cooldownTimer <= 0)
        {
            if (ability == 1)
            {
                type.AbilityOne();
                cooldownTimer = abilityCooldownOne;
            }
            if (ability == 2)
            {
                type.AbilityTwo();
                cooldownTimer = abilityCooldownTwo;
            }
        }
    }

    public void ClearTarget()
    {
        target = null;
    }

    public void LoseHP(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        foreach(EnemyUnit e in targetedBy)
        {
            e.ClearTarget();
            e.targetedBy.Remove(gameObject.GetComponent<PlayerUnit>());
        }
        if(tag == "Melee")
        {
            gameManager.meleeUnits.Remove(gameObject);
        }
        if (tag == "Ranged")
        {
            gameManager.rangedUnits.Remove(gameObject);
        }
        if (tag == "Support")
        {
            gameManager.supportUnits.Remove(gameObject);
        }
        Destroy(gameObject);
    }
}
