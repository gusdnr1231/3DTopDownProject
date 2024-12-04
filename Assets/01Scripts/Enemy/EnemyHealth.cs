using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IEnemyCompomnent, IDamageable
{
	public float maxHealth;
	[SerializeField] private float _currentHealth;

	public event Action OnHit;
	public event Action OnDead;

	private EnemyBase _enemy;

	public void Initilize(EnemyBase enemy)
	{
		_enemy = enemy;
		_currentHealth = maxHealth;
	}

public void ApplyDamage(float damage)
	{
		_currentHealth -= damage;
		_currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
		OnHit?.Invoke();

		if(Mathf.Approximately(_currentHealth, 0))
		{
			OnDead?.Invoke();
		}
	}

	public void AfterInitilize()
	{
		throw new NotImplementedException();
	}
}
