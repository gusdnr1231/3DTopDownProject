using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
	public WeaponDataSO weaponData;
	public int bulletInMagazine;

	public Transform GunTrm { get; set; }
	public Transform GunPoint { get; set; }

	protected float _nextShootTime;
	protected float _currentSpread, _lastSpreadTime;

	public void Shooting()
	{
		bulletInMagazine--;
		_nextShootTime = Time.time + 1 / weaponData.fireRate;
	}

	public void AdjustRig(Transform leftTarget, Transform leftHint)
	{
		leftTarget.localPosition = weaponData.leftHandPosition;
		leftTarget.localRotation = Quaternion.Euler(weaponData.leftHandRotation);
		leftHint.localPosition = weaponData.leftHandHintPosition;
	}

	#region Bullet Spread Region
	public void UpdateSpread()
	{
		if(Time.time > _lastSpreadTime + weaponData.spreadCooldown)
		{
			_currentSpread = weaponData.spreadAmount;
		}
		else IncreaseSpread();
		_lastSpreadTime = Time.time;
	}

	public void IncreaseSpread()
	{
		_currentSpread = Mathf.Clamp(_currentSpread + weaponData.spreadIncRate, weaponData.spreadAmount, weaponData.maxSpreadAmount);
	}

	public Vector3 ApplySpread(Vector3 originalDirection)
	{
		UpdateSpread();

		float randomize = Random.Range(-_currentSpread, _currentSpread);

		Quaternion spreadRotation = Quaternion.Euler(randomize, randomize, randomize);
		return spreadRotation * originalDirection;
	}

	#endregion

	#region Reloading Ammo Abstract
	public abstract bool CanReload();
	public abstract void TryToReloadBullet();
	public abstract void FillBullet();
	#endregion

	public abstract bool CanShooting();
}
