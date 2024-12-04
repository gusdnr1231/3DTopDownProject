using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyLowCondition : Conditional
{
    public float lowValue = 5f;
    public SharedFloat energy;

	public override TaskStatus OnUpdate()
	{
		if(energy.Value < lowValue)
		{
			return TaskStatus.Success;
		}
		else
		{
			return TaskStatus.Failure;
		}
	}
}
