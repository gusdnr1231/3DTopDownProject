using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponModel : MonoBehaviour
{
    public MeleeWeaponType weaponType;
    public AnimatorOverrideController animController;
    public List<MeleeAttackDataSO> atkDataList;

    public void SetActiveModel(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
