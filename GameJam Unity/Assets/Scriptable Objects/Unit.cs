using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit", order = 1)]
public class Unit : ScriptableObject
{
    public int hp;
    public int damage;
    public float range;
    public float fireRate;
    public float abilityCooldownOne;
    public float abilityCooldownTwo;

    public virtual void AbilityOne()
    {
        Debug.Log("Ability one activated");
    }

    public virtual void AbilityTwo()
    {
        Debug.Log("Ability two activated");
    }
}
