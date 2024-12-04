using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTargetAction : Action
{
    public SharedEnemyBase enemy;
    public bool IsAcitveAutoRotate;

	private EnemyMovement _movement;

	public override void OnAwake()
	{
		_movement = enemy.Value.GetCompo<EnemyMovement>();
	}

	public override TaskStatus OnUpdate()
	{
		_movement.SetAutoRotate(IsAcitveAutoRotate);
		return TaskStatus.Success;
	}
}
