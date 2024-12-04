using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimerAction : Action
{
    public SharedFloat startTime;

	public override void OnStart()
	{
		startTime.Value = Time.time;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;	
	}
}
