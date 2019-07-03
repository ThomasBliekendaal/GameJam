using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyUnit : MonoBehaviour
{
    [Header ("Fill Manually")]
    public AIUnit type;
    public SphereCollider attackTrigger;
    public GameObject popUp;
    public Color red;

    [Header("Data")]
    public List<PlayerUnit> seenBy = new List<PlayerUnit>();
    public List<PlayerUnit> targetedBy = new List<PlayerUnit>();
    public List<Minion> targetedByMinions = new List<Minion>();
    private GameManager gameManager;
    private NavMeshAgent agent;
    private GameObject target;

    [SerializeField] private int hp;
    private int damage;
    private float attackRange;
    private float detectRange;
    private float fireRate;
    private float fireTimer;
    private bool attacking;
    private bool stay;

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
        attackTrigger.radius = attackRange;
        TargetSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            stay = false;
            TargetSelect();
        }
        else if (stay == false)
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
        if (target.tag == "Minion")
        {
            target.GetComponent<Minion>().hp -= damage;
        }
        else
        {
            target.GetComponent<PlayerUnit>().LoseHP(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Minion")
        {
            target = other.gameObject;
            target.GetComponent<Minion>().targetedBy.Add(gameObject.GetComponent<EnemyUnit>());
            attacking = true;
        }
        if(other.gameObject == target.gameObject)
        {
            attacking = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == target.gameObject)
        {
            agent.destination = transform.position;
            stay = true;
        }
    }

    public void LoseHP(int i)
    {
        GameObject g = Instantiate(popUp, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0), Quaternion.identity);
        g.GetComponent<TextMeshProUGUI>().color = red;
        g.GetComponent<PopUp>().text = "-" + i.ToString();
        hp -= i;
        if(hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        gameManager.enemyUnits.Remove(gameObject);
        foreach(PlayerUnit p in targetedBy)
        {
            p.ClearTarget();
            p.targetedBy.Remove(gameObject.GetComponent<EnemyUnit>());
        }
        foreach (PlayerUnit p in seenBy)
        {
            p.enemiesInRange.Remove(gameObject.GetComponent<EnemyUnit>());
        }
        foreach (Minion m in targetedByMinions)
        {
            m.ResetTarget();
        }
        gameManager.GetComponent<UIManager>().points += 1;
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
