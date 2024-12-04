using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeChase : EnemyState
{
	private float _lastUpdateTime;
	private EnemyMovement _movement;
	private MeleeWeaponController _weaponController;

	public EnemyMeleeChase(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
		_movement = enemy.GetCompo<EnemyMovement>();
		_weaponController = enemy.GetCompo<MeleeWeaponController>();
	}

	public override void Enter()
	{
		base.Enter();
		_movement.SetStop(false);	
		_movement.SetSpeed(_enemy.chaseSpeed);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		_enemy.FaceToTarget(_movement.GetNextPathPoint());

		if(_weaponController.IsPlayerInAttackRange())
		{
			_stateMachine.ChangeState(_enemy.GetState(MeleeState.Attack));
			return;
		}

		if (CanUpdateDestionation())
		{
			_movement.SetDestination(_enemy.TargetTrm.position);
		}
	}

	private bool CanUpdateDestionation()
	{
		float delay = 0.5f;
		if(Time.time > _lastUpdateTime + delay)
		{
			_lastUpdateTime = Time.time;
			return true;
		}

		return false;
	}
}
