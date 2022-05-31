using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObject/Weapon")]
public class BulletData : ScriptableObject
{
    public float bulletSpeed;
    public float maxDistance;
}
