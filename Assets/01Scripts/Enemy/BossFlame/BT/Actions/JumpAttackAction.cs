using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackAction : Action
{
    public SharedTransform target;
    public SharedEnemyBase enemy;
    public SharedBool isAnimationEnd;

    public float jumpTime = 1f;

    private EnemyMovement _movement;
    private Vector3 _targetPosition;
    private float _moveSpeed;

	public override void OnAwake()
	{
		_movement = enemy.Value.GetCompo<EnemyMovement>();
	}

	public override void OnStart()
	{
		_targetPosition = target.Value.position;

		float distance = Vector3.Distance(_targetPosition, transform.position);
		_moveSpeed = distance / jumpTime;
		isAnimationEnd.Value = false;
	}

	public override TaskStatus OnUpdate()
	{
		if (_movement.IsManualMovement)
		{
			transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
		}

		if (isAnimationEnd.Value)
		{
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}
}
