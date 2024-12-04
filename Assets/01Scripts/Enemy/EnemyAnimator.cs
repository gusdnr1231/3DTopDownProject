using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IEnemyCompomnent
{
    private EnemyBase _enemy;
    private Animator _animator;
	public Animator Animator => _animator;

	private EnemyMovement _movement;

	private readonly int _atkAnimSpeedHash = Animator.StringToHash("AtkAnimationSpeed");
	private readonly int _atkIndexHash = Animator.StringToHash("AtkIndex");
	private readonly int _slashIndexHash = Animator.StringToHash("SlashIndex");
	private readonly int _recoveryIndexHash = Animator.StringToHash("RecoveryIndex");
	private readonly int _chaseIndexHash = Animator.StringToHash("ChaseIndex");
	private readonly int _dodgeRollTriggerHash = Animator.StringToHash("DodgeRoll");

	public void Initilize(EnemyBase enemy)
	{
		_enemy = enemy;
        _animator = GetComponent<Animator>();
		_movement = enemy.GetCompo<EnemyMovement>();
	}

	public void SetAnimatorControllerIfExist(AnimatorOverrideController animController)
	{
		if (_animator == null)
		{
			return;
		}

		_animator.runtimeAnimatorController = animController;
	}

	public void SetAttackParam(int atkIndex, int slashIndex, float animationSpeed)
	{
		_animator.SetInteger(_atkIndexHash, atkIndex);
		_animator.SetInteger(_slashIndexHash, slashIndex);
		_animator.SetFloat(_atkAnimSpeedHash, animationSpeed);
	}

	private void AnimationEnd()
	{
		_enemy.OnAnimationEnd();
	}

	private void StartManualMovement() => _movement.SetManualMovement(true);
	private void StopManualMovement() => _movement.SetManualMovement(false);

	private void StartManualRotation() => _movement.SetManualRotation(false);
	private void StopManualRotation() => _movement.SetManualRotation(false);

	public void SetRecoveryIndex(int index) => _animator.SetInteger(_recoveryIndexHash, index);
	public void SetChaseIndex(float index) => _animator.SetFloat(_chaseIndexHash, index);

	public void AfterInitilize()
	{
	}

	public void DodgeRoll()
	{
		_animator.SetTrigger(_dodgeRollTriggerHash);
	}
}
