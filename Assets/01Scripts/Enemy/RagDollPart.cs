using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollPart : MonoBehaviour, IDamageable, IKnokBackable, IHasAbility
{
	private Rigidbody _rbCompo;
	private Collider _colliderCompo;

	public event Action<float> OnDamageEvent;
	public event Action OnTriggerAblilityEvent;

	private void Awake()
	{
		_rbCompo = GetComponent<Rigidbody>();
		_colliderCompo = GetComponent<Collider>();

		SetRagDollActive(false);
	}

	public void SetRagDollActive(bool isActive)
	{
		_rbCompo.isKinematic = !isActive;
	}

	public void SetCollider(bool isActive)
	{
		_colliderCompo.enabled = isActive;
	}

	public void ApplyDamage(float damage)
	{
		OnDamageEvent?.Invoke(damage);
	}

	public void KnokBack(Vector3 force, Vector3 point)
	{
		DOVirtual.DelayedCall(0.1f, () =>
		{
			if(_rbCompo.isKinematic) return;
			_rbCompo.AddForceAtPosition(force, point, ForceMode.Impulse);
		});
	}

	public void TriggerAbility()
	{
		OnTriggerAblilityEvent?.Invoke();
	}
}
