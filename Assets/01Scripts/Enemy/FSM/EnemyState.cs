using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy _enemy;
    protected EnemyStateMachine _stateMachine;
    protected int _animBoolHash;

    protected float _stateTimer;
    protected bool _animationTrigger;
    protected Animator _anim;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
		_animBoolHash = Animator.StringToHash(animBoolName);
        _anim = _enemy.GetCompo<EnemyAnimator>().Animator;

	}

    public virtual void Enter()
    {
        _animationTrigger = false;
        _anim.SetBool(_animBoolHash, true);
    }

    public virtual void UpdateState()
    {
        _stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        _anim.SetBool(_animBoolHash, false);
    }

    public void SetAnimationTrigger()
    {
        _animationTrigger = true;
    }

}
