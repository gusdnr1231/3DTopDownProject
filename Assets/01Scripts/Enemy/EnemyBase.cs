using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
	[Header("Move Data")]
	public float moveSpeed;
	public float turnSpeed;
	public float chaseSpeed;

	[Header("Target Info")]
	[SerializeField] protected PlayerManagerSO _playerManager;
	public Player Target => _playerManager.Player;
	public Transform TargetTrm => _playerManager.PlayerTrm;

	public bool IsDead { get; protected set; } = false;

	protected Dictionary<Type, IEnemyCompomnent> _components;

	protected virtual void Awake()
	{
		_components = new Dictionary<Type, IEnemyCompomnent>();
		GetComponentsInChildren<IEnemyCompomnent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

		_components.Values.ToList().ForEach(compo => compo.Initilize(this));

		_components.Values.ToList().ForEach(compo => compo.AfterInitilize());
	}

	public void FaceToTarget(Vector3 target)
	{
		Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
		Vector3 eulerAngles = transform.rotation.eulerAngles;

		float yRot = Mathf.LerpAngle(eulerAngles.y, targetRot.eulerAngles.y, turnSpeed * Time.deltaTime);

		transform.rotation = Quaternion.Euler(eulerAngles.x, yRot, eulerAngles.z);
	}

	public T GetCompo<T>() where T : class
	{
		if (_components.TryGetValue(typeof(T), out IEnemyCompomnent compo))
		{
			return compo as T;
		}

		return default;
	}

	public abstract void OnAnimationEnd();
}

public class SharedEnemyBase : SharedVariable<EnemyBase>
{
	public static implicit operator SharedEnemyBase(EnemyBase value)
	{
		return new SharedEnemyBase{Value = value };
	}
}