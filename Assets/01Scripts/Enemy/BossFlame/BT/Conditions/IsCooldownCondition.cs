using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCooldownCondition : Conditional
{
    public SharedFloat cooldown;

	public override TaskStatus OnUpdate()
	{
		if(cooldown.Value > 0) return TaskStatus.Success;
		return TaskStatus.Failure;
	}
}
