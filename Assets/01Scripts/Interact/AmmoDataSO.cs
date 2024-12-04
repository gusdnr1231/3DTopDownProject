using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/Ammo")]
public class AmmoDataSO : ScriptableObject
{
	public WeaponDataSO weaponType;
	public int minAmount, maxAmount;

	public int GetRandomAmount()
	{
		return Random.Range(minAmount, maxAmount);
	}
}
