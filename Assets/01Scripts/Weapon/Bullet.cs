using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPayload
{
	public float mass;
	public Vector3 velocity;
	public float shootingRange;
	public float impactForce;
	public float damage;
}

public class Bullet : MonoBehaviour, IPoolable
{
	protected float _maxDistacne;
	protected Vector3 _startPosition;
	public Rigidbody RbCompo { get;  protected set; }

	[field:SerializeField] public PoolTypeSO PoolType { get; private set; }
	public GameObject GameObject => gameObject;

	private Pool _myPool; 

	protected float _impactForce, _damage;
	protected TrailRenderer _trailRenderer;

	[SerializeField] private GameEventChannelSO _spawnChannel;
	[SerializeField] private PoolTypeSO _impactType;

	protected virtual void Awake()
	{
		_trailRenderer = GetComponent<TrailRenderer>();
		RbCompo = GetComponent<Rigidbody>();
	}

	protected virtual void Update()
	{	
		float distacne = Vector3.Distance(transform.position, _startPosition);
		if(distacne > _maxDistacne * 0.6f)
		{
			_trailRenderer.time -= 2 * Time.deltaTime;
		}

		if(distacne >= _maxDistacne)
		{
			_myPool.Push(this);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		CreateImpactVFX(collision);

		if(collision.gameObject.TryGetComponent(out IDamageable damageable))
		{
			damageable.ApplyDamage(_damage);
		}

		if (collision.gameObject.TryGetComponent(out IKnokBackable knokbackable))
		{
			Vector3 force = RbCompo.velocity.normalized * _impactForce;
			Vector3 hitPoint = collision.contacts[0].point;
			knokbackable.KnokBack(force, hitPoint);
		}

		_myPool.Push(this);
	}

	private void CreateImpactVFX(Collision other)
	{
		if(other.contacts.Length > 0)
		{
			ContactPoint contact = other.contacts[0];
			var evt = SpawnEvents.EffectSpawn;
			evt.effectType = _impactType;
			evt.point = contact.point;
			evt.rotation = Quaternion.LookRotation(contact.normal);
			_spawnChannel.RaiseEvent(evt);
		}
	}

	public void Fire(Vector3 position, Quaternion rotation, BulletPayload payload)
	{
		transform.SetPositionAndRotation(position, rotation);
		_trailRenderer.Clear();
		RbCompo.mass = payload.mass;
		RbCompo.velocity = payload.velocity;
		_maxDistacne = payload.shootingRange;
		_impactForce = payload.impactForce;
		_damage = payload.damage;
		_startPosition = position;
	}

	public void SetUpPool(Pool pool)
	{
		_myPool = pool;
	}

	public void ResetItem()
	{
		_trailRenderer.time = 0.25f;
	}
}
