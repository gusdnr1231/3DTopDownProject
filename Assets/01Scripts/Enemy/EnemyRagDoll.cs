using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagDoll : MonoBehaviour, IEnemyCompomnent
{
	private EnemyBase _enemy;
	private EnemyHealth _health;

	[SerializeField] private Transform _ragDollParent;
	[SerializeField] private RagDollPart[] _ragDollParts;

	public void Initilize(EnemyBase enemy)
	{
		_enemy = enemy;
		_health = _enemy.GetCompo<EnemyHealth>();
		_ragDollParts = _ragDollParent.GetComponentsInChildren<RagDollPart>();
	}

	private void AddListener()
	{
		foreach(RagDollPart part in _ragDollParts)
		{
			part.OnDamageEvent += (damageAmount) =>
			{
				_health.ApplyDamage(damageAmount);
			};
			part.OnTriggerAblilityEvent += HandleTriggerAbility;
		}
	}

	private void HandleTriggerAbility()
	{
		if(_enemy is Enemy enemy) enemy.TriggerAblility();
	}

	public void SetRagDollActive(bool isActive)
	{
		foreach (RagDollPart part in _ragDollParts)
		{
			part.SetRagDollActive(isActive);
		}
	}

	public void SetColliderActive(bool isActive)
	{
		foreach (RagDollPart part in _ragDollParts)
		{
			part.SetCollider(isActive);
		}
	}

	public void AfterInitilize()
	{
		AddListener();
	}
}
