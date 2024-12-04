using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationAction : Action
{
    public SharedEnemyBase enemy;

    public AnimatorParamSO param;
    private BossAnimator _bossAnimator;

	public override void OnAwake()
	{
		_bossAnimator = enemy.Value.GetCompo<BossAnimator>();
	}

	public override TaskStatus OnUpdate()
	{
		_bossAnimator.ClearAllBoolen();
		_bossAnimator.SetParam(param, new ParamValue{boolValue = true});
		return TaskStatus.Success;
	}
}
