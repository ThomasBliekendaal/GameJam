using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Support Unit", menuName = "Support Unit", order = 4)]
public class SupportUnit : Unit
{
    public int healAmount;
    public float damageMultiplier;
    public float damageBuffTime;

    public override void AbilityOne(GameObject self)
    {
        foreach (PlayerUnit p in self.gameObject.GetComponent<PlayerUnit>().units)
        {
            p.LoseHP(healAmount);
        }
    }

    public override void AbilityTwo(GameObject self)
    {
        foreach (PlayerUnit p in self.gameObject.GetComponent<PlayerUnit>().units)
        {
            p.damageMultiplier = damageMultiplier;
            p.damageBuffTimer = damageBuffTime;
            p.damageBuff = true;
        }
    }
}
