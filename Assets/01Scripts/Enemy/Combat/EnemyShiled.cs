using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShiled : MonoBehaviour, IDamageable
{
	public event Action OnDestroyShiled;

	private Rigidbody _rbCompo;
	[SerializeField] private float _durability;

	private void Awake()
	{
		_rbCompo = GetComponent<Rigidbody>();
	}

	public void DropShiled()
	{
		transform.SetParent(null);
		_rbCompo.isKinematic = false;
	}

	public void ApplyDamage(float damage)
	{
		_durability -= damage;
		if( _durability <= 0 )
		{
			OnDestroyShiled?.Invoke();
			gameObject.SetActive(false);
		}
	}
}
