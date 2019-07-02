﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    [Header("Fill manually")]
    public Unit type;
    public SphereCollider rangeTrigger;

    [Header("Abilities")]
    public float invulnarableTime;
    public float spinTime;
    public bool damageBuff;
    public float damageMultiplier;
    public float damageBuffTimer;

    [Header("Data")]
    public GameObject target;
    public GameObject endGoal;
    private NavMeshAgent agent;
    private GameManager gameManager;
    private float fireTimer;
    public List<EnemyUnit> enemiesInRange = new List<EnemyUnit>();
    public List<EnemyUnit> targetedBy = new List<EnemyUnit>();
    public List<PlayerUnit> units = new List<PlayerUnit>();
    [SerializeField] private PlayerUnit healTarget;

    [SerializeField] private int hp;
    private int damage;
    private float range;
    private float fireRate;
    private float abilityCooldownOne;
    private float abilityCooldownTwo;
    private float cooldownTimer;

    [Header("UI")]
    public Button abilityOne;
    public Button abilityTwo;

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
        invulnarableTime = 0;
    }

    void Start()
    {
        rangeTrigger.radius = range;
        endGoal = gameManager.endGoals[Random.Range(0, gameManager.endGoals.Count)];
        agent.destination = endGoal.transform.position;
        abilityOne.onClick.AddListener(delegate { Ability(1); });
        abilityTwo.onClick.AddListener(delegate { Ability(2); });
        foreach(GameObject g in gameManager.meleeUnits)
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
        foreach (GameObject g in gameManager.rangedUnits)
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
        foreach (GameObject g in gameManager.supportUnits)
        {
            units.Add(g.GetComponent<PlayerUnit>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            agent.destination = gameObject.transform.position;
            transform.LookAt(target.transform.position);
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0 && spinTime <= 0)
            {
                Attack();
                fireTimer = fireRate;
            }
            else if (spinTime > 0)
            {

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

        if(invulnarableTime > 0)
        {
            invulnarableTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            enemiesInRange.Add(other.GetComponent<EnemyUnit>());
            other.gameObject.GetComponent<EnemyUnit>().seenBy.Add(this.gameObject.GetComponent<PlayerUnit>());
        }
        if(target == null && other.tag == "Enemy")
        {
            target = other.gameObject;
            other.gameObject.GetComponent<EnemyUnit>().targetedBy.Add(this.gameObject.GetComponent<PlayerUnit>());
        }
    }

    public void Attack()
    {
        if (gameObject.tag != "Support")
        {
            if (damageBuff == false)
            {
                if (spinTime <= 0)
                {
                    target.GetComponent<EnemyUnit>().LoseHP(damage);
                }
                else
                {
                    foreach(EnemyUnit e in enemiesInRange)
                    {
                        e.LoseHP(damage);
                        spinTime -= Time.deltaTime;
                    }
                }
            }
            else
            {
                if (spinTime <= 0)
                {
                    target.GetComponent<EnemyUnit>().LoseHP(Mathf.CeilToInt(damage * damageMultiplier));
                }
                else
                {
                    foreach (EnemyUnit e in enemiesInRange)
                    {
                        target.GetComponent<EnemyUnit>().LoseHP(Mathf.CeilToInt(damage * damageMultiplier));
                        spinTime -= Time.deltaTime;
                    }
                }

                damageBuffTimer -= Time.deltaTime;
                if(damageBuffTimer <= 0)
                {
                    damageBuff = false;
                }
            }
        }
        else
        {
            Debug.Log("Priest");
            foreach(PlayerUnit p in units)
            {
                if(healTarget == null && p.hp < p.type.hp)
                {
                    healTarget = p;
                }
                else if((p.type.hp - p.hp) > (healTarget.type.hp - healTarget.hp))
                {
                    healTarget = p;
                }
            }
            if (healTarget != null)
            {
                Debug.Log("heal");
                healTarget.LoseHP(-damage);
                healTarget = null;
            }
            else
            {
                target.GetComponent<EnemyUnit>().LoseHP(damage);
            }
        }
    }

    public virtual void Ability(int ability)
    {
        if (cooldownTimer <= 0)
        {
            if (ability == 1)
            {
                type.AbilityOne(gameObject);
                cooldownTimer = abilityCooldownOne;
            }
            if (ability == 2)
            {
                type.AbilityTwo(gameObject);
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
        if (invulnarableTime <= 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Death();
            }
        }
        if(hp > type.hp)
        {
            hp = type.hp;
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
