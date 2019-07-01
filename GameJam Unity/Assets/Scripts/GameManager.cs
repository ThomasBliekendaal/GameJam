using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> endGoals = new List<GameObject>();
    public List<GameObject> meleeUnits = new List<GameObject>();
    public List<GameObject> rangedUnits = new List<GameObject>();
    public List<GameObject> supportUnits = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Melee"))
        {
            meleeUnits.Add(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Ranged"))
        {
            rangedUnits.Add(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Support"))
        {
            supportUnits.Add(g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
