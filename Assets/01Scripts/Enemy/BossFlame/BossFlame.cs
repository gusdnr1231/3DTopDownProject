using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BossFlame : EnemyBase
{
	private BehaviorTree _tree;
	private SharedBool _isAnimationEnd;
	private SharedFloat _energy;
	private SharedFloat _cooldown;
	private SharedTransform _target;

	public float maxEnergy;

	protected override void Awake()
	{
		base.Awake();

		_tree = GetComponent<BehaviorTree>();

		_isAnimationEnd = _tree.GetVariable("isAnimationEnd") as SharedBool;
		_energy = _tree.GetVariable("energy") as SharedFloat;
		_cooldown = _tree.GetVariable("cooldown") as SharedFloat;
		_target = _tree.GetVariable("target") as SharedTransform;

		_energy.Value = maxEnergy;
	}

	private void Start()
	{
		_target.Value = _playerManager.PlayerTrm;
	}

	private void Update()
	{
		if(_cooldown.Value > 0)
		{
			_cooldown.Value -= Time.deltaTime;
		}
	}

	public override void OnAnimationEnd()
	{
		Debug.Log("Boss Animation End");
		_isAnimationEnd.Value = true;
	}
}
