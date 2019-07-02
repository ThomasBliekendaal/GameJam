using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Unit", menuName = "Ranged Unit", order = 3)]
public class RangedUnit : Unit
{
    public GameObject explosion;
    public GameObject minion;
    public List<Vector3> spawnOffsets = new List<Vector3>();

    public override void AbilityOne(GameObject self)
    {
        Debug.Log("explosion");
        Instantiate(explosion, self.GetComponent<PlayerUnit>().target.transform.position, Quaternion.identity);
    }

    public override void AbilityTwo(GameObject self)
    {
        Debug.Log("summon");
        foreach(Vector3 v in spawnOffsets)
        {
            Instantiate(minion, self.transform.position + v, self.transform.rotation);
        }
    }
}
