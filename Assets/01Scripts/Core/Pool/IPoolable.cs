using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
	public PoolTypeSO PoolType { get; }
	public GameObject GameObject { get; }
	public void SetUpPool(Pool pool);
	public void ResetItem();
}
