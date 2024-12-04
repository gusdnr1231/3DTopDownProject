using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlameAttackCompo : MonoBehaviour, IEnemyCompomnent
{
	private BossFlame _boss;
	[SerializeField] private ParticleSystem[] _fireExplosion;
	[SerializeField] private Transform _leftFirePos, _rightFirePos;
	[SerializeField] private GameEventChannelSO _spawnChannel;

	public void Initilize(EnemyBase enemy)
	{
		_boss = enemy as BossFlame;
	}

	public void AfterInitilize()
	{
	}

	public void ShootingGreanade()
	{
		var evt = SpawnEvents.GrenadeCreate;
		Vector3 targetPos = _boss.TargetTrm.position;

		foreach(var effet in _fireExplosion)
		{
			effet.Play();
			evt.angle = 50f;
			evt.targetPos = targetPos;
			evt.firePos = _leftFirePos.position;
			_spawnChannel.RaiseEvent(evt);
			evt.firePos = _rightFirePos.position;
			_spawnChannel.RaiseEvent(evt);
		}
	}
}
