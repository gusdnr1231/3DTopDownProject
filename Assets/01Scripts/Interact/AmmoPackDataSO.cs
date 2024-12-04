using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType
{
	Small, Big
}

[CreateAssetMenu(menuName = "SO/Weapon/AmmoPack")]
public class AmmoPackDataSO : ScriptableObject
{
    public AmmoBoxType boxType;
	public List<AmmoDataSO> ammoList;
}
