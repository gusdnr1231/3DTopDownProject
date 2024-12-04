using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParamValue
{
    public bool boolValue;
    public float floatValue;
    public int intValue;
}

public class BossAnimator : MonoBehaviour, IEnemyCompomnent
{
	public bool IsManualMove {  get; private set; }
	[SerializeField] private AnimatorParamListSO _paramList;

	private BossFlame _boss;
	private Animator _animator;
	private EnemyMovement _movement;
	private BossFlameAttackCompo _attackCompo;

	public void Initilize(EnemyBase enemy)
	{
		_boss = enemy as BossFlame;
		_animator = GetComponent<Animator>();
		_movement = enemy.GetCompo<EnemyMovement>();
		_attackCompo = enemy.GetCompo<BossFlameAttackCompo>();
	}

	public void AfterInitilize()
	{
	}

	private void AnimationEnd()
	{
		_boss.OnAnimationEnd();
	}

	public void GrenadeTrigger() => _attackCompo.ShootingGreanade();

	public void ClearAllBoolen() => _paramList.ClearAllBoolen(_animator);

	public void SetParam(AnimatorParamSO param, ParamValue value)
	{
		switch (param.paramType)
		{
			case ParamType.Boolen:
				_animator.SetBool(param.hashValue, value.boolValue); break;
			case ParamType.Float:
				_animator.SetFloat(param.hashValue, value.floatValue); break;
			case ParamType.Trigger:
				_animator.SetTrigger(param.hashValue); break;
			case ParamType.Integer:
				_animator.SetInteger(param.hashValue, value.intValue); break;
		}
	}


	public void SetManualMove() => _movement.SetManualMovement(true);
	public void StopManualMove() => _movement.SetManualMovement(false);
}
