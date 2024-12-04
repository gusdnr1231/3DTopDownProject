using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MeleeState
{
	Idle, Move, Recovery, Chase, Attack, Dead
}

public enum MeleeWeaponType
{
	OneHand, UnArmed
}

public enum MeleeAddOn
{
	Regular, Shiled, Dodge
}

public class EnemyMelee : Enemy
{
	[Header("AddOn Data")]
	public MeleeAddOn addOnType;
	[SerializeField] private float _abilityCooldown = 2f;
	private float _lastUseAbilityTime;

	private Dictionary<MeleeState, EnemyState> _states;

	protected override void Awake()
	{
		base.Awake();
		_states = new Dictionary<MeleeState, EnemyState>()
		{
			{MeleeState.Idle, new EnemyMeleeIdle(this, StateMachine, "Idle") },
			{MeleeState.Move, new EnemyMeleeMove(this, StateMachine, "Move") },
			{MeleeState.Recovery, new EnemyMeleeRecovery(this, StateMachine, "Recovery") },
			{MeleeState.Chase, new EnemyMeleeChase(this, StateMachine, "Chase") },
			{MeleeState.Attack, new EnemyMeleeAttack(this, StateMachine, "Attack") },
			{MeleeState.Dead, new EnemyMeleeDead(this, StateMachine, "Idle") }
		};
	}

	protected override void Start()
	{
		base.Start();
		StateMachine.Initialize(_states[MeleeState.Idle]);
	}

	protected override void Update()
	{
		base.Update();
		if (Keyboard.current.pKey.wasPressedThisFrame)
		{
			StateMachine.ChangeState(_states[MeleeState.Dead]);
		}
	}

	public override EnemyState GetState(Enum enumType)
	{
		var state = (MeleeState) enumType;
		if(_states.TryGetValue(state, out EnemyState enemyState))
		{
			return enemyState;
		}

		return null;
	}

	public override void EnteringBattleMode()
	{
		if(IsDead) return;

		PlayerSpotted = true;
		GetCompo<EnemyAnimator>().SetRecoveryIndex(0);
		StateMachine.ChangeState(_states[MeleeState.Recovery]);
	}

	public override void SetDead()
	{
		base.SetDead();

		StateMachine.ChangeState(_states[MeleeState.Dead]);
	}

	public override void TriggerAblility()
	{
		if(addOnType != MeleeAddOn.Dodge || StateMachine.CurrentState != _states[MeleeState.Chase]) return;

		if(_lastUseAbilityTime + _abilityCooldown > Time.time) return;

		float distance = Vector3.Distance(transform.position, TargetTrm.position);

		if(distance < 3f) return;

		_lastUseAbilityTime = Time.time;
		GetCompo<EnemyAnimator>().DodgeRoll();
	}
}
