using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeMove : EnemyState
{
	private Vector3 _destination;
	private EnemyMovement _movement;

	public EnemyMeleeMove(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
		_movement = enemy.GetCompo<EnemyMovement>();
	}

	public override void Enter()
	{
		base.Enter();
		_movement.SetSpeed(_enemy.moveSpeed);

		_destination = _enemy.GetPatrolPosition();
		_movement.SetDestination(_destination);
	}

	public override void UpdateState()
	{
		base.UpdateState();

		_enemy.FaceToTarget(_movement.GetNextPathPoint());

		if (_movement.IsArrived)
		{
			_stateMachine.ChangeState(_enemy.GetState(MeleeState.Idle));
		}
	}
}
