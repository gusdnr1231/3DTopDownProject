using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyState
{
	private Vector3 _attackDestination;
	private Vector3 _startPosition;
	private EnemyMelee _enemyMelee;

	private const float MAX_ATK_DISTANCE = 50f;

	private EnemyMovement _movement;
	private EnemyAnimator _animator;
	private MeleeWeaponController _weaponController;

	public EnemyMeleeAttack(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
		_enemyMelee = enemy as EnemyMelee;
		_animator = enemy.GetCompo<EnemyAnimator>();
		_movement = enemy.GetCompo<EnemyMovement>();
		_weaponController = enemy.GetCompo<MeleeWeaponController>();
	}

	public override void Enter()
	{
		base.Enter();
		MeleeAttackDataSO atkData = _weaponController.attackData;
		_animator.SetAttackParam(atkData.atkIndex, atkData.slashIndex, atkData.animationSpeed);

		_movement.SetStop(true);
		_movement.SetVelocity(Vector3.zero);
		_startPosition = _enemy.transform.position;

		UpdateDestination();
	}

	private void UpdateDestination()
	{
		Vector3 moveDelta = _enemy.transform.forward * MAX_ATK_DISTANCE;
		_attackDestination = _startPosition + moveDelta;
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if (_movement.IsManualRotation)
		{
			UpdateDestination();
			_enemy.FaceToTarget(_enemy.TargetTrm.position);
		}

		if (_movement.IsManualMovement)
		{
			_enemy.transform.position = Vector3.MoveTowards(
				_enemy.transform.position,
				_attackDestination,
				_weaponController.attackData.moveSpeed * Time.deltaTime);
		}

		if (_animationTrigger)
		{
			_stateMachine.ChangeState(_enemy.GetState(MeleeState.Recovery));
			return;
		}
	}

	public override void Exit()
	{
		_weaponController.UpdateNextAttack();
		base.Exit();
	}
}
