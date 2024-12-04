using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeAttackType
{
    Close, Charge
}

[CreateAssetMenu(menuName = "SO/Enemy/MeleeAttackData")]
public class MeleeAttackDataSO : ScriptableObject
{
    public string atkName;
    public float atkRange;
    public float moveSpeed;
    public int atkIndex;
    public int slashIndex;
    [Range(1,3f)] public float animationSpeed;
    public MeleeAttackType atkType;
}
