using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDead : EnemyState
{
	private EnemyMovement _movement;
	private EnemyRagDoll _ragDoll;

	public EnemyMeleeDead(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
		_movement = enemy.GetCompo<EnemyMovement>();
		_ragDoll = enemy.GetCompo<EnemyRagDoll>();
	}

	public override void Enter()
	{
		base.Enter();
		_anim.enabled = false;
		_movement.SetStop(true);
		_ragDoll.SetRagDollActive(true);

		DOVirtual.DelayedCall(5f, () =>
		{
			_ragDoll.SetColliderActive(false);
			_ragDoll.SetRagDollActive(false);

			//오브젝트 풀링은 이곳에서
		});
	}
}
