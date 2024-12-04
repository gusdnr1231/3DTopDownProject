using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnergyAction : Action
{
    public SharedFloat energy;
    public float maxEnergy;
    public float chargingTime;

	public override TaskStatus OnUpdate()
	{
		energy.Value += Time.deltaTime * chargingTime;

		if(energy.Value >= maxEnergy)
		{
			energy.Value = maxEnergy;
			return TaskStatus.Success;
		}

		return TaskStatus.Running;
	}
}
