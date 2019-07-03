using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject victory;
    public GameObject gameOver;

    public List<GameObject> endGoals = new List<GameObject>();
    public List<GameObject> meleeUnits = new List<GameObject>();
    public List<GameObject> rangedUnits = new List<GameObject>();
    public List<GameObject> supportUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    private int playerUnitCount;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Melee"))
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
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyUnits.Add(g);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerUnitCount = meleeUnits.Count + rangedUnits.Count + supportUnits.Count;
        if(playerUnitCount <= 0)
        {
            gameOver.SetActive(true);
        }
        else if (enemyUnits.Count <= 0)
        {
            victory.SetActive(true);
        }
    }
}
