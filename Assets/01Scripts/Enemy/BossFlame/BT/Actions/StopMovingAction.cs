using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingAction : Action
{
    public SharedEnemyBase enemy;
    private EnemyMovement _movement;

	public override void OnStart()
	{
		if(_movement == null) _movement = enemy.Value.GetCompo<EnemyMovement>();
	}

	public override TaskStatus OnUpdate()
	{
		_movement.SetStop(true);
		_movement.SetVelocity(Vector3.zero);
		return TaskStatus.Success;
	}
}
