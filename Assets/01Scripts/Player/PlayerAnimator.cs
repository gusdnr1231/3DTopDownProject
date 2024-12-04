using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : MonoBehaviour, IPlayerComponent
{
	[SerializeField] private MultiAimConstraint _aimIK;
	[SerializeField] private TwoBoneIKConstraint _leftHandIK;

	private readonly int _xVelocityHash = Animator.StringToHash("xVelocity");
	private readonly int _zVelocityHash = Animator.StringToHash("zVelocity");
	private readonly int _isRunningHash = Animator.StringToHash("isRunning");
	private readonly int _fireTriggerHash = Animator.StringToHash("fire");
	private readonly int _grabTypeHash = Animator.StringToHash("weaponGrabType");
	private readonly int _grabTriggerHash = Animator.StringToHash("weaponGrab");
	private readonly int _equipSpeedHash = Animator.StringToHash("equipSpeed");
	private readonly int _reloadTriggerHash = Animator.StringToHash("reload");
	private readonly int _reloadSpeedHash = Animator.StringToHash("reloadSpeed");

	private Animator _animator;
	private Player _player;

	private PlayerMovement _moveCompo;
	private bool _isPlayRunAnimation;

	private PlayerWeaponController _weaponController;

	public event Action<bool> GrabStatusChangeEvent;
	public event Action WeaponGrabTriggerEvent;
	public event Action<bool> ReloadStatusChangeEvent;

	public void Initialize(Player player)
	{
		_player = player;

		_animator = GetComponent<Animator>();
		_moveCompo = _player.GetCompo<PlayerMovement>();
		_moveCompo.OnMovement += HandleMovement;
		_moveCompo.OnRunning += HandleOnRunning;

		_weaponController = _player.GetCompo<PlayerWeaponController>();
		_weaponController.WeaponFireEvent += HandleWeaponFireEvent;
		_weaponController.WeaponChangeStartEvent += HandleWeaponChangeStart;
		_weaponController.ReloadEvent += HandleWeaponReloadStart;
	}

	private void HandleWeaponReloadStart(float reloadSpeed)
	{
		AimWeightTween(0, 0.2f, 0);
		LeftHandWeightTween(0, 0.2f, 0);
		
		ReloadStatusChangeEvent?.Invoke(true);

		_animator.SetFloat(_reloadSpeedHash, reloadSpeed);
		_animator.SetTrigger(_reloadTriggerHash);
	}

	private void ReloadAnimationEnd()
	{
		AimWeightTween(1, 0.2f, 0);
		LeftHandWeightTween(1, 0.2f, 0);

		ReloadStatusChangeEvent?.Invoke(false);
	}

	private void SwitchAnimationLayer(int layerIndex)
	{
		for(int i = 1; i < _animator.layerCount; i++)
		{
			_animator.SetLayerWeight(i, 0);
		}
		_animator.SetLayerWeight(layerIndex, 1);
	}

	private void PlayGrabAnimation(GrabType grabType, float equipSpeed)
	{
		GrabStatusChangeEvent?.Invoke(false);

		AimWeightTween(0, 0.5f, 0);
		LeftHandWeightTween(0, 0.5f, 0);

		_animator.SetFloat(_equipSpeedHash, equipSpeed);
		_animator.SetInteger(_grabTypeHash, (int)grabType);
		_animator.SetTrigger(_grabTriggerHash);
	}

	public void ChangeWeaponAnimation()
	{
		WeaponGrabTriggerEvent?.Invoke();

		AimWeightTween(1, 0.5f, 0.1f);
		LeftHandWeightTween(1, 0.5f, 0.5f);
	}
	public void GrabAnimationEnd() => GrabStatusChangeEvent?.Invoke(true);

	public void AimWeightTween(float endValue, float time, float delay, Action OnComplete = null)
	{
		DOTween.To(() => _aimIK.weight, value => _aimIK.weight = value, endValue, time)
			.SetDelay(delay).OnComplete(() => OnComplete?.Invoke());
	}

	public void LeftHandWeightTween(float endValue, float time, float delay, Action OnComplete = null)
	{
		DOTween.To(() => _leftHandIK.weight, value => _leftHandIK.weight = value, endValue, time)
			.SetDelay(delay).OnComplete(() => OnComplete?.Invoke());
	}

	private void HandleWeaponChangeStart(PlayerWeapon before, PlayerWeapon next)
	{
		int layerIndex = next.weaponData.animationLayer;
		SwitchAnimationLayer(layerIndex);
		PlayGrabAnimation(next.weaponData.grabType, next.weaponData.equipSpeed);
	}

	private void HandleWeaponFireEvent()
	{
		_animator.SetTrigger(_fireTriggerHash);
	}

	private void HandleOnRunning(bool isRunning)
	{
		_isPlayRunAnimation = isRunning;
	}

	private void HandleMovement(Vector3 movement)
	{
		float x = Vector3.Dot(movement, transform.right);
		float z = Vector3.Dot(movement, transform.forward);

		float dampTime = 0.1f;

		_animator.SetFloat(_xVelocityHash, x, dampTime, Time.deltaTime);
		_animator.SetFloat(_zVelocityHash, z, dampTime, Time.deltaTime);

		bool isPlay = movement.sqrMagnitude > 0 && _isPlayRunAnimation;

		_animator.SetBool(_isRunningHash, isPlay);
	}
}
