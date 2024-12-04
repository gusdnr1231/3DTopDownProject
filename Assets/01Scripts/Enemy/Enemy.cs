using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : EnemyBase
{
    [Header("Idle info")]
    public float idleTime;
    public float aggressiveRange;

    [Header("Patrol Info")]
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolIdx;

    public bool PlayerSpotted { get; protected set; }
    private bool _isEnterBattleMode;

    public EnemyStateMachine StateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();

		GetCompo<EnemyHealth>().OnDead += SetDead;
		GetCompo<EnemyHealth>().OnHit += HandleOnHit;
	}

    protected virtual void Start()
    {
        foreach(var point in _patrolPoints) point.SetParent(null);
    }

	private void HandleOnHit()
	{
		if (PlayerSpotted == false)
        {
            EnteringBattleMode();
        }
	}

	protected virtual void Update()
    {
        if(StateMachine != null)
        {
            StateMachine.UpdateMachine();
        }

        CheckEnterBattleMode();
    }

	public override void OnAnimationEnd()
	{
		StateMachine.CurrentState.SetAnimationTrigger();
	}

	private void CheckEnterBattleMode()
    {
        if(_isEnterBattleMode == false && IsPlayerInAggressiveRange())
        {
            _isEnterBattleMode = true;
            EnteringBattleMode();
        }
    }

    private bool IsPlayerInAggressiveRange() => Vector3.Distance(TargetTrm.position, transform.position) < aggressiveRange;
	public virtual void SetDead()
    {
        IsDead = true;  
    }
    
    public Vector3 GetPatrolPosition()
    {
        Vector3 point = _patrolPoints[_currentPatrolIdx].position;
        _currentPatrolIdx = (_currentPatrolIdx + 1) % _patrolPoints.Length;
        return point;
    }

    public abstract EnemyState GetState(Enum enumType);
    public abstract void EnteringBattleMode();
    public abstract void TriggerAblility();

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressiveRange);
        Gizmos.color = Color.white;
	}
#endif
}
