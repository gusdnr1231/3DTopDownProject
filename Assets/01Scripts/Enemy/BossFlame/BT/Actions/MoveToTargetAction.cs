using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetAction : Action
{
    public SharedEnemyBase enemy;
    public SharedTransform target;

    public float reCalcPeriod = 0.5f;

    private EnemyMovement _movement;
    private float _lastCalcTime;

	public override void OnAwake()
	{
		_lastCalcTime = Time.time;

		_movement = enemy.Value.GetCompo<EnemyMovement>();

        _movement.SetSpeed(enemy.Value.moveSpeed);
        _movement.SetStop(false);
	}

	public override void OnStart()
	{
		_movement.SetDestination(target.Value.position);
	}

	public override TaskStatus OnUpdate()
	{
		enemy.Value.FaceToTarget(_movement.GetNextPathPoint());
		if(_lastCalcTime + reCalcPeriod < Time.time)
		{
			_movement.SetDestination(target.Value.position);
			_lastCalcTime = Time.time;
		}

		return TaskStatus.Running;
	}
}
