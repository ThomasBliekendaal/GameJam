using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public Unit type;

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
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
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
}
