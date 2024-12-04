using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _poolManager;

	private void Awake()
	{
		_poolManager.InitializePool(transform);
	}
}
