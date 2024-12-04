using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private PoolManagerSO _poolManager;	
	[SerializeField] private PoolTypeSO _bulletType, _grenadeType;

	private void Start()
	{
		_spawnChannel.AddListener<PlayerBulletCreate>(HandlePlayerBulletCreate);
		_spawnChannel.AddListener<EffectSpawn>(HandleEffectSpawn);
		_spawnChannel.AddListener<GrenadeCreate>(HandleGrenadeCreate);
	}

	private void HandleGrenadeCreate(GrenadeCreate evt)
	{
		var grenade = _poolManager.Pop(_grenadeType) as Grenade;
		grenade.FireGrenade(evt.angle, evt.firePos, evt.targetPos);
	}

	private void HandlePlayerBulletCreate(PlayerBulletCreate evt)
	{
		Bullet bullet = _poolManager.Pop(_bulletType) as Bullet;
		bullet.Fire(evt.position, evt.rotation, evt.payload);
	}

	private void HandleEffectSpawn(EffectSpawn evt)
	{
		var effect = _poolManager.Pop(evt.effectType) as EffectPlayer;
		effect.PlayEffect(evt.point, evt.rotation);
	}
}
