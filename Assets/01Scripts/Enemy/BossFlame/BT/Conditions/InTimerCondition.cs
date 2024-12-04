using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTimerCondition : Conditional
{
    public SharedFloat startTime;
    public float waitTime;

	public override TaskStatus OnUpdate()
	{
		if(startTime.Value + waitTime < Time.time)
		{
			return TaskStatus.Failure;
		}
		return TaskStatus.Success;
	}
}
