using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeIdle : EnemyState
{
	public EnemyMeleeIdle(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		_stateTimer = _enemy.idleTime;
	}

	public override void UpdateState()
	{
		base.UpdateState();

		if(_stateTimer < 0)
		{
			_stateMachine.ChangeState(_enemy.GetState(MeleeState.Move));
		}
	}
}
