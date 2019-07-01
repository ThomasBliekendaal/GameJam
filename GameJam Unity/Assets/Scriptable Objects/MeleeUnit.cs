using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Unit", menuName = "Melee Unit", order = 2)]
public class MeleeUnit : Unit
{
    public float invulnerableTime;

    public override void AbilityOne(GameObject self)
    {
        self.GetComponent<PlayerUnit>().invulnarableTime = invulnerableTime;
    }
}
