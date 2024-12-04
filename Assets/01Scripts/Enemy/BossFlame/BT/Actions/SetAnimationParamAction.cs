using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationParamAction : Action
{
	public SharedEnemyBase enemy;
	public AnimatorParamSO param;
	public ParamValue value;

	private BossAnimator _animator;

	public override void OnAwake()
	{
		_animator = enemy.Value.GetCompo<BossAnimator>();
	}

	public override TaskStatus OnUpdate()
	{
		_animator.SetParam(param, value);
		return TaskStatus.Success;
	}
}
