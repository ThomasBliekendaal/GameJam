using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Unit", menuName = "Ranged Unit", order = 3)]
public class RangedUnit : Unit
{
    public GameObject explosion;
    public GameObject minion;
    public Vector3 summonOffset;

    public override void AbilityOne(GameObject self)
    {
        Instantiate(explosion, self.GetComponent<PlayerUnit>().target.transform.position, Quaternion.identity);
    }

    public override void AbilityTwo(GameObject self)
    {
        Instantiate(minion, self.transform.position + summonOffset, self.transform.rotation);
    }
}
