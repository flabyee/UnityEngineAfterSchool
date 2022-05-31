using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStat", menuName = "ScriptableObject/Zombie")]
public class ZombieStat : ScriptableObject
{
    public int attackPower;
    public int score;
    public int maxHP;
    public float speed;
    public float animationSpeed;
    public float attackDelay;
    public ZombieType zombieType;
}
