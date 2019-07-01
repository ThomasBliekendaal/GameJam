using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Unit", menuName = "AI Unit", order = 5)]
public class AIUnit : ScriptableObject
{
    public int hp;
    public int damage;
    public float attackRange;
    public float detectRange;
    public float fireRate;
}
