using UnityEngine;

public class EnemyMeleeRecovery : EnemyState
{
	private EnemyMovement _movement;
	private MeleeWeaponController _weaponController;
	private EnemyAnimator _animator;

	public EnemyMeleeRecovery(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
	{
		_movement = enemy.GetCompo<EnemyMovement>();
		_weaponController = enemy.GetCompo<MeleeWeaponController>();
		_animator = enemy.GetCompo<EnemyAnimator>();
	}

	public override void Enter()
	{
		base.Enter();
		_movement.SetStop(true);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		_enemy.FaceToTarget(_enemy.TargetTrm.position);

		if (_animationTrigger)
		{
			if (_weaponController.IsPlayerInAttackRange()) _stateMachine.ChangeState(_enemy.GetState(MeleeState.Attack));
			else _stateMachine.ChangeState(_enemy.GetState(MeleeState.Chase));

			return;
		}
	}

	public override void Exit()
	{
		_movement.SetStop(false);
		_animator.SetRecoveryIndex(1);
		base.Exit();
	}

}
