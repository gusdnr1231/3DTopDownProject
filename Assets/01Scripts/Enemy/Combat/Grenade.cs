using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IPoolable
{
	[SerializeField] private PoolTypeSO _poolType;

	public PoolTypeSO PoolType => _poolType;

	public GameObject GameObject => gameObject;

	private Rigidbody _rbCompo;
	private float _gravity;
	private Pool _myPool;

	private void Awake()
	{
		_rbCompo = GetComponent<Rigidbody>();
		_gravity = Physics.gravity.magnitude;
	}

	public void FireGrenade(float fireAngle, Vector3 firePos, Vector3 targetPos)
	{
		transform.position = firePos;
		float angle = fireAngle * Mathf.Deg2Rad; //라디안 각으로 바꿔

		Vector3 planeTarget = new Vector3(targetPos.x, 0, targetPos.z);
		Vector3 planePosition = new Vector3(firePos.x, 0, firePos.z); //y를 제거하고 

		float distance = Vector3.Distance(planeTarget, planePosition); //XZ 평면상에서 거리
		float yOffset = firePos.y - targetPos.y;

		//초기 속도구하기 식
		float startVelocity = (1f / Mathf.Cos(angle))
							* Mathf.Sqrt((0.5f * _gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
		Vector3 velocity = new Vector3(0, startVelocity * Mathf.Sin(angle), startVelocity * Mathf.Cos(angle));

		//2차원속도를 3차원으로 전환
		Vector3 planeDirection = planeTarget - planePosition;
		float angleBetweenObjects = Vector3.Angle(Vector3.forward, planeDirection);
		Vector3 cross = Vector3.Cross(Vector3.forward, planeDirection);

		if (cross.y < 0)
		{
			angleBetweenObjects *= -1;
		}

		Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

		_rbCompo.AddForce(finalVelocity * _rbCompo.mass, ForceMode.Impulse);
	}

	public void ResetItem()
	{
		throw new System.NotImplementedException();
	}

	public void SetUpPool(Pool pool)
	{
		_myPool = pool;
	}

	private void OnCollisionEnter(Collision collision)
	{
		_rbCompo.velocity = Vector3.zero;
		_myPool.Push(this);
	}
}
