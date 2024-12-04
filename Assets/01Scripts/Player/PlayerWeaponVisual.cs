using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponVisual : MonoBehaviour, IPlayerComponent
{
	[Header("IK Settings")]
	[SerializeField] private Transform _leftHandTarget, _leftHandHint;

	private Player _player;
	private PlayerWeapon _nextWeapon;

	public void Initialize(Player player)
	{
		_player = player;
		_player.GetCompo<PlayerWeaponController>().WeaponChangeStartEvent += HandleWeaponChangeStart;
		_player.GetCompo<PlayerAnimator>().WeaponGrabTriggerEvent += HandleChnageWeaponTrigger;
	}

	private void HandleWeaponChangeStart(PlayerWeapon before, PlayerWeapon next)
	{
		if(before != null && before.weaponData != null)
		{
			before.ActiveGun(false);
		}
		_nextWeapon = next;
	}

	private void HandleChnageWeaponTrigger()
	{
		_nextWeapon.ActiveGun(true);
		_nextWeapon.AdjustRig(_leftHandTarget, _leftHandHint);
	}
}
