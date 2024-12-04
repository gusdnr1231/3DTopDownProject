using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatAction : Action
{
	public enum Operation
	{
		Assign, Add, Sub
	}

	public SharedFloat target;
	public Operation op;
	public float value;

	public override TaskStatus OnUpdate()
	{
		switch (op)
		{
			case Operation.Assign:
				target.Value = value; break;
			case Operation.Add:
				target.Value += value; break;
			case Operation.Sub:
				target.Value -= value; break;
		}

		return TaskStatus.Success;
	}
}
