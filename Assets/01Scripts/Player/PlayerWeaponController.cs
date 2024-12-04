using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour, IPlayerComponent
{
	public delegate void WeaponChange (PlayerWeapon before, PlayerWeapon next);

	[Header("WeaponData")]
	[SerializeField] private List<PlayerWeapon> _weaponSlots;
	[HideInInspector] public PlayerWeapon currentWeapon;
	[SerializeField] private int _maxGunSlotAllowed = 2;
	public Transform GunPonitTrm => currentWeapon.GunPoint;
	[SerializeField] private PickUpWeapon _pickUpWeaponPrefab;

	[Space]
	[SerializeField] private LayerMask _whatIsEnemy;

	[Header("Weapon Holder")]
	[SerializeField] private Transform _weaponHolderTrm;
	[SerializeField] private Transform _backHolderTrm;
	[SerializeField] private Transform _sideHolderTrm;

    private Player _player;
	public event Action<float> ReloadEvent;
	public event Action WeaponFireEvent;
	public event WeaponChange WeaponChangeStartEvent;

	private bool _isShooting, _weaponReady;

	[Header("Event Channel")]
	[SerializeField] private GameEventChannelSO _spawnEvents;
	[SerializeField] private GameEventChannelSO _valueEvents;
	private BulletPayload _payload;

	public void Initialize(Player player)
	{
		_player = player;
		var inputCompo = _player.GetCompo<InputReaderSO>();
		inputCompo.FireEvent += HandleFireInputEvent;
		inputCompo.ChangeWeaponSlotEvent += HandleChangeWeaponSlot;
		inputCompo.ReloadEvent += HandleReloadEvent;

		var animCompo = _player.GetCompo<PlayerAnimator>();
		animCompo.GrabStatusChangeEvent += HandleGrabStatusChange;
		animCompo.ReloadStatusChangeEvent += HandleReloadStatusChange;

		foreach (PlayerWeapon weapon in _weaponSlots)
		{
			weapon.SetUpGun(_weaponHolderTrm, _backHolderTrm, _sideHolderTrm);
			weapon.bulletInMagazine = weapon.weaponData.maxAmmo;
		}

		_weaponReady = true;

		_payload = new BulletPayload();
	}

	private void HandleReloadStatusChange(bool isReloading)
	{
		if(currentWeapon == null) return;
		_weaponReady = !isReloading;

		if(isReloading)
		{
			currentWeapon.FillBullet();
		}
	}

	private void Start()
	{
		HandleChangeWeaponSlot(0);
	}

	private void HandleReloadEvent()
	{
		if(_weaponReady && currentWeapon.CanReload())
		{
			ReloadEvent?.Invoke(currentWeapon.weaponData.reloadSpeed);
			currentWeapon.TryToReloadBullet();
		}
	}

	private void HandleGrabStatusChange(bool isGrabWeapon)
	{
		_weaponReady = isGrabWeapon;
		if (_weaponReady == true)
		{
			var evt = ValueEvents.CamDistanceEvent;
			evt.distance = currentWeapon != null ? currentWeapon.weaponData.camDistance: 5f;
			_valueEvents.RaiseEvent(evt);
		}
	}

	private void HandleChangeWeaponSlot(int slotIndex)
	{
		if(slotIndex >= _weaponSlots.Count || slotIndex < 0) return;
		if (_weaponSlots[slotIndex].weaponData == null) return;
		if(_weaponReady == false) return;

		PlayerWeapon before = currentWeapon;
		currentWeapon = _weaponSlots[slotIndex];

		WeaponChangeStartEvent?.Invoke(before, currentWeapon);
	}

	private void HandleFireInputEvent(bool isFire)
	{
		_isShooting = isFire;
	}

	private void Update()
	{
		if (_isShooting)
		{
			Shooting();
		}
	}

	private void Shooting()
	{
		if(_weaponReady == false || currentWeapon.CanShooting() == false) return;
		WeaponFireEvent?.Invoke();

		currentWeapon.Shooting();
		if(currentWeapon.weaponData.shootType == ShootType.Single)
		{
			_isShooting = false;
		}

		for(int i = 0; i < currentWeapon.weaponData.bulletPerShot; i++)
		{
			FireSingleBullet();
		}

		TriggerEnemyAbility();
	}

	private void TriggerEnemyAbility()
	{
		Vector3 rayOrigin = GunPonitTrm.position;
		Vector3 rayDirection = _player.GetCompo<PlayerAim>().GetBulletDirection(GunPonitTrm);

		if(Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity, _whatIsEnemy))
		{
			if(hit.collider.TryGetComponent(out IHasAbility ability))
			{
				ability.TriggerAbility();
			}
		}
	}

	private void FireSingleBullet()
	{
		SetUpBulletPayload();

		var evt = SpawnEvents.PlayerBulletCreate;
		evt.position = currentWeapon.GunPoint.position;
		evt.rotation = currentWeapon.GunPoint.rotation;
		evt.payload = _payload;

		_spawnEvents.RaiseEvent(evt);
	}

	private void SetUpBulletPayload()
	{
		var data = currentWeapon.weaponData;
		Vector3 bulletDirection = _player.GetCompo<PlayerAim>().GetBulletDirection(currentWeapon.GunPoint);

		_payload.mass = 2f / data.bulletSpeed;
		_payload.shootingRange = data.shootingRange;
		_payload.impactForce = data.impactForce;
		_payload.damage = data.damage;
		_payload.velocity = currentWeapon.ApplySpread(bulletDirection) * data.bulletSpeed;
	}

	public void PickUpAmmoPack(AmmoPackDataSO packDataSO)
	{
		foreach (var ammo in packDataSO.ammoList)
		{
			foreach(PlayerWeapon weapon in _weaponSlots)
			{
				if(weapon.weaponData == ammo.weaponType)
					weapon.reservedAmmo += ammo.GetRandomAmount();
			}
		}
	}

	public void PickUpWeapon(PickUpWeapon pickUpWeapon)
	{
		PlayerWeapon hasWeapon = GetWeaponInSlots(pickUpWeapon.WeaponData);
        if (hasWeapon != null)
        {
            hasWeapon.reservedAmmo += pickUpWeapon.ReservedAmmo;
			return;
        }

		if(_weaponSlots.Count >= _maxGunSlotAllowed)
		{
			int index = _weaponSlots.IndexOf(currentWeapon);
			int newIndex = AddWeapon(pickUpWeapon);

			(_weaponSlots[index], _weaponSlots[newIndex]) = (_weaponSlots[newIndex], _weaponSlots[index]);

			DropWeapon(index);
			return;
		}

		AddWeapon(pickUpWeapon);
    }

	private void DropWeapon(int nextEquipIndex)
	{
		if (_weaponSlots.Count <= 1) return;

		PickUpWeapon dropWeapon = Instantiate(_pickUpWeaponPrefab);
		dropWeapon.SetUpWeaponData(currentWeapon);

		_weaponSlots.Remove(currentWeapon);

		HandleChangeWeaponSlot(nextEquipIndex);
	}

	private int AddWeapon(PickUpWeapon pickUpWeapon)
	{
		PlayerWeapon newWeapon = new PlayerWeapon() { weaponData = pickUpWeapon.WeaponData };
		newWeapon.SetUpGun(_weaponHolderTrm, _backHolderTrm, _sideHolderTrm);
		_weaponSlots.Add(newWeapon);

		newWeapon.bulletInMagazine = pickUpWeapon.AmmoInMagazine;
		newWeapon.reservedAmmo = pickUpWeapon.ReservedAmmo;
		return _weaponSlots.Count - 1;
	}

	private PlayerWeapon GetWeaponInSlots(WeaponDataSO weaponData)
	{
		return _weaponSlots.FirstOrDefault(w => w.weaponData == weaponData);
	}
}
