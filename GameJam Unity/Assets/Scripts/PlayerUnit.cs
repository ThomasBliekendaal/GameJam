using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class PlayerUnit : MonoBehaviour
{
    [Header("Fill manually")]
    public Unit type;
    public SphereCollider rangeTrigger;

    [Header("UI Fill manually")]
    public Button abilityOne;
    public Button abilityTwo;
    public Button displayUpgrades;
    public GameObject upgrades;
    public Button addHP;
    public Button addDamage;
    public Button addSpeed;
    public Button addFireRate;
    public GameObject popUp;
    public Color red;
    public Color green;
    public Text hpText;
    public Text damageText;
    public Text speedText;
    public Text fireRateText;

    [Header("Abilities")]
    public float invulnarableTime;
    public float spinTime;
    public bool damageBuff;
    public float damageMultiplier;
    public float damageBuffTimer;

    [Header("Animation")]
    private Animator anim;
    private IEnumerator coroutine;

    [Header("Data")]
    public GameObject target;
    [SerializeField] public GameObject endGoal;
    private NavMeshAgent agent;
    [SerializeField] private GameManager gameManager;
    private float fireTimer;
    private float hitTimer;
    public List<EnemyUnit> enemiesInRange = new List<EnemyUnit>();
    public List<EnemyUnit> targetedBy = new List<EnemyUnit>();
    public List<PlayerUnit> units = new List<PlayerUnit>();
    private PlayerUnit healTarget;
    private AudioSource source;
    private GameObject particles;

    private int hp;
    private int maxHp;
    private int damage;
    private float range;
    private float fireRate;
    private float hitDelay;
    private float abilityCooldownOne;
    private float abilityCooldownTwo;
    private float cooldownTimer;
    

    // Start is called before the first frame update

    private void Awake()
    {
        coroutine = Attacking();
        hp = type.hp;
        maxHp = type.hp;
        damage = type.damage;
        range = type.range;
        fireRate = type.fireRate;
        hitDelay = type.hitDelay;
        abilityCooldownOne = type.abilityCooldownOne;
        abilityCooldownTwo = type.abilityCooldownTwo;
        cooldownTimer = 0;
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        invulnarableTime = 0;
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if(gameObject.tag == "Melee")
        {
            particles = gameObject.GetComponentInChildren<ParticleSystem>().gameObject;
        }
    }

    void Start()
    {
        rangeTrigger.radius = range;
        print(gameManager.endGoals.Count);
        endGoal = gameManager.endGoals[Random.Range(0, gameManager.endGoals.Count)];
        if(endGoal == null)
        {
            endGoal = GameObject.FindGameObjectWithTag("End");
        }
        agent.destination = endGoal.transform.position;
        SetAnim("Walking");
        if(displayUpgrades != null)
        {
            displayUpgrades.onClick.AddListener(DisplayUpgrades);
        }
        if(addHP != null)
        {
            addHP.onClick.AddListener(AddHp);
        }
        if(addDamage != null)
        {
            addDamage.onClick.AddListener(AddDamage);
        }
        if(addSpeed != null)
        {
            addSpeed.onClick.AddListener(AddSpeed);
        }
        if(addFireRate != null)
        {
            addFireRate.onClick.AddListener(AddFireRate);
        }
        if(abilityOne != null)
        {
            abilityOne.onClick.AddListener(delegate { Ability(1); });
        }
        if (abilityTwo != null)
        {
            abilityTwo.onClick.AddListener(delegate { Ability(2); });
        }
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
        UpdateStatUI();
        if(particles != null)
        {
            particles.SetActive(false);
            gameObject.GetComponent<ParticleBaker>().UpdateMesh(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.destination = gameObject.transform.position;
            transform.LookAt(target.transform.position);
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0 && spinTime <= 0)
            {
                coroutine = Attacking();
                SetAnim("Attacking");
                //source.PlayOneShot(type.attack);
                fireTimer = fireRate;
                //Attack();
                StartCoroutine(coroutine);
            }
        }
        else
        {
            SetAnim("Walking");
            agent.destination = endGoal.transform.position;
        }

        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(invulnarableTime > 0)
        {
            if(particles != null)
            {
                particles.SetActive(true);
                gameObject.GetComponent<ParticleBaker>().UpdateMesh(true);
            }
            invulnarableTime -= Time.deltaTime;
        }
        else
        {
            if(particles != null)
            {
                particles.SetActive(false);
                gameObject.GetComponent<ParticleBaker>().UpdateMesh(false);
            }
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
            SetAnim("Walking");
            target = other.gameObject;
            other.gameObject.GetComponent<EnemyUnit>().targetedBy.Add(this.gameObject.GetComponent<PlayerUnit>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(spinTime > 0 && other.tag == "Enemy" && fireTimer <= 0)
        {
            if(damageBuff == true)
            {
                other.GetComponent<EnemyUnit>().LoseHP(Mathf.CeilToInt(damage * damageMultiplier));
                fireTimer = fireRate;
            }
            else
            {
                other.GetComponent<EnemyUnit>().LoseHP(damage);
                fireTimer = fireRate;
            }
        }
    }

    public void Attack()
    {
        SetAnim("Attacking");
        if (gameObject.tag != "Support")
        {
            if (damageBuff == false)
            {
                if (spinTime <= 0)
                {
                    target.GetComponent<EnemyUnit>().LoseHP(damage);
                    //fireTimer = fireRate;
                }
            }
            else
            {
                if (fireTimer <= 0 && spinTime <= 0)
                {
                    target.GetComponent<EnemyUnit>().LoseHP(Mathf.CeilToInt(damage * damageMultiplier));
                    //fireTimer = fireRate;
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
            foreach(PlayerUnit p in units)
            {
                if (healTarget == null && p.hp < p.maxHp)
                {
                    healTarget = p;
                }
                else if(healTarget != null)
                {
                    if((p.maxHp - p.hp) > (healTarget.maxHp - healTarget.hp))
                    {
                        healTarget = p;
                    }
                }
            }
            if (healTarget != null)
            {
                healTarget.LoseHP(-damage);
                healTarget = null;
                source.PlayOneShot(type.healAudio);
                //fireTimer = fireRate;
            }
            else
            {
                target.GetComponent<EnemyUnit>().LoseHP(damage);
                //fireTimer = fireRate;
            }
        }
    }

    public virtual void Ability(int ability)
    {
        if (cooldownTimer <= 0)
        {
            source.PlayOneShot(type.buttonPress);
            if (ability == 1)
            {
                type.AbilityOne(gameObject);
                source.PlayOneShot(type.abilityOneAudio);
                cooldownTimer = abilityCooldownOne;
            }
            if (ability == 2)
            {
                type.AbilityTwo(gameObject);
                source.PlayOneShot(type.abilityTwoAudio);
                cooldownTimer = abilityCooldownTwo;
            }
        }
        else
        {
            source.PlayOneShot(type.buttonFail);
        }
    }

    public void ClearTarget()
    {
        target = null;
    }

    public void LoseHP(int damage)
    {
        //SetAnim("Flinching");
        if (invulnarableTime <= 0)
        {

            GameObject g = Instantiate(popUp, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0), Quaternion.identity);
            if (damage > 0)
            {
                g.GetComponent<TextMeshProUGUI>().color = red;
                g.GetComponent<PopUp>().text = "-" + damage.ToString();
                source.PlayOneShot(type.getHit);
            }
            else if (damage < 0)
            {
                GameObject o = Instantiate(type.healVFX, transform.position, Quaternion.identity);
                o.transform.rotation.SetEulerAngles(-90, 0, 0);
                g.GetComponent<TextMeshProUGUI>().color = green;
                g.GetComponent<PopUp>().text = "+" + (-1 * damage).ToString();
                source.PlayOneShot(type.getHeal);
            }
            g.transform.position = transform.position + new Vector3(0, 1, 0);

            hp -= damage;
            if (hp <= 0)
            {
                Death();
            }
        }
        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }

    public void Death()
    {
        SetAnim("Dying");
        if (type.name == "MeleeUnit")
        {
            gameManager.meleeUnits.Remove(gameObject);
        }
        if (type.name == "RangedUnit")
        {
            gameManager.rangedUnits.Remove(gameObject);
        }
        if (type.name == "SupportUnit")
        {
            gameManager.supportUnits.Remove(gameObject);
        }
        foreach (EnemyUnit e in targetedBy)
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
        source.PlayOneShot(type.death);
        Destroy(gameObject);
    }

    private void DisplayUpgrades()
    {
        upgrades.SetActive(!upgrades.active);
        UpdateStatUI();
    }

    private void AddHp()
    {
        if(gameManager.GetComponent<UIManager>().points > 0)
        {
            gameManager.GetComponent<UIManager>().points -= 1;
            hp += 10;
            maxHp += 10;
        }
        UpdateStatUI();
    }

    public void AddDamage()
    {
        if (gameManager.GetComponent<UIManager>().points > 0)
        {
            gameManager.GetComponent<UIManager>().points -= 1;
            damage += 1;
        }
        UpdateStatUI();
    }

    public void AddSpeed()
    {
        if (gameManager.GetComponent<UIManager>().points > 0)
        {
            gameManager.GetComponent<UIManager>().points -= 1;
            agent.speed += 1;
        }
        UpdateStatUI();
    }

    public void AddFireRate()
    {
        if (gameManager.GetComponent<UIManager>().points > 0 && fireRate > 0.3)
        {
            gameManager.GetComponent<UIManager>().points -= 1;
            fireRate -= 0.2f;
        }
        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        hpText.text = maxHp.ToString();
        damageText.text = damage.ToString();
        speedText.text = agent.speed.ToString();
        fireRateText.text = fireRate.ToString();
    }

    public void SetAnim(string animation)
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFlinching", false);
        anim.SetBool("isBuffed", false);
        anim.SetBool("isDying", false);
        if (animation == "Idle")
        {
            anim.SetBool("isIdle", true);
        }
        if (animation == "Walking")
        {
            anim.SetBool("isWalking", true);
        }
        if (animation == "Attacking")
        {
            anim.SetBool("isAttacking", true);
        }
        if (animation == "Flinching")
        {
            anim.SetBool("isFlinching", true);
        }
        if (animation == "Spinning")
        {
            anim.SetBool("isSpinning", true);
        }
        if (animation == "Buffed")
        {
            anim.SetBool("isBuffed", true);
        }
        if (animation == "Dying")
        {
            anim.SetBool("isDying", true);
        }
    }

    private IEnumerator Attacking()
    {
        GameObject o = Instantiate(type.attackVFX, target.transform.position, Quaternion.identity);
        o.transform.rotation.SetEulerAngles(-90, 0, 0);
        source.PlayOneShot(type.attack);
        Attack();
        yield return new WaitForSeconds(68 * Time.deltaTime);
    }
}
