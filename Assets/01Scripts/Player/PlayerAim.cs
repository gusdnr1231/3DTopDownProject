using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour, IPlayerComponent
{
	[Header("Aim Control")]
	[SerializeField] private Transform _aimTrm;
	[field: SerializeField] public bool IsAimPrecisely { get; private set; }
	[field: SerializeField] public bool IsLockingToTarget { get; private set; }
	[SerializeField] private float _playerHeight = 1.35f;
	public Transform AimTarget { get; private set; }

	[Header("Camera Aim Info")]
	[Range(0.5f, 1f)][SerializeField] private float _minCamDistance;
	[Range(1f, 4.5f)][SerializeField] private float _maxCamDistance = 3.5f;
	[Range(3f, 5f)][SerializeField] private float _camSensitivity = 5f;
	[SerializeField] private Transform _camTargetTrm;

	[Header("Aim Visual")]
	[SerializeField] private LineRenderer _aimLaser;
	[SerializeField] private float _laserDistance = 4f;

	public event Action<Quaternion> OnLookDirectionChange;

	private InputReaderSO _inputCompo;

	private Player _player;
	private Vector3 _mousePos;
	private Vector3 _beforeLookDirection;

	public void Initialize(Player player)
	{
		_player = player;

		_inputCompo = _player.GetCompo<InputReaderSO>();

		_aimLaser = transform.Find("AimLaser").GetComponent<LineRenderer>();
		_aimLaser.positionCount = 4;
		_aimLaser.enabled = false;

		var animCompo = _player.GetCompo<PlayerAnimator>();
		animCompo.GrabStatusChangeEvent += HandleGrapStatusChange;
		animCompo.ReloadStatusChangeEvent += HandleReloadStatusChange;
	}

	private void Update()
	{
		UpdateAimPosition();
		UpdateLookDirection();

		UpdateAimLaser();
	}

	private void UpdateAimPosition()
	{
		_mousePos = _inputCompo.GetWorldMousePosition();

		_camTargetTrm.position = Vector3.Lerp(_camTargetTrm.position, GetDesiredCamPosition(), _camSensitivity * Time.deltaTime);

		if (IsLockingToTarget)
		{
			AimTarget = GetTarget();
			if (AimTarget != null)
			{
				if (AimTarget.TryGetComponent(out Renderer renderer))
				{
					_aimTrm.position = renderer.bounds.center;
				}
				else
				{
					_aimTrm.position = AimTarget.position;
				}
				return;
			}
		}

		if (IsAimPrecisely) _aimTrm.position = _mousePos;
		else
		{
			_aimTrm.position = new Vector3(_mousePos.x, transform.position.y + _playerHeight, _mousePos.z);
		}
	}

	private Vector3 GetDesiredCamPosition()
	{
		bool isMoveDown = _inputCompo.Movement.y < -0.5f;
		float actualMaxCamDistance = isMoveDown ? _minCamDistance : _maxCamDistance;

		Vector3 aimDirection = _mousePos - transform.position;
		float distance = Vector3.Distance(transform.position, _mousePos);
		distance = Mathf.Clamp(distance, _minCamDistance, actualMaxCamDistance);

		Vector3 desiredPosition = transform.position + aimDirection.normalized * distance;
		desiredPosition.y = transform.position.y + _playerHeight;

		return desiredPosition;
	}

	private Transform GetTarget()
	{
		Transform target = null;
		RaycastHit hit = _inputCompo.GetMouseHitInfo();
		if (hit.collider != null)
		{
			target = hit.transform;
		}
		return target;
	}

	private void UpdateLookDirection()
	{
		Vector3 lookDirection = _mousePos - transform.position;
		lookDirection.y = 0;
		lookDirection.Normalize();
		if (_beforeLookDirection != lookDirection)
		{
			OnLookDirectionChange?.Invoke(Quaternion.LookRotation(lookDirection));
			_beforeLookDirection = lookDirection;
		}
	}

	public Vector3 GetBulletDirection(Transform gunPointTrm)
	{
		Vector3 direction = (_aimTrm.position - gunPointTrm.position).normalized;
		if (IsAimPrecisely == false && AimTarget == null)
		{
			direction.y = 0;
		}
		return direction;
	}

	private void UpdateAimLaser()
	{
		Transform gunPointTrm = _player.GetCompo<PlayerWeaponController>().GunPonitTrm;

		if (gunPointTrm == null) return;

		Vector3 start = gunPointTrm.position;
		Vector3 laserDirection = GetBulletDirection(gunPointTrm).normalized;
		Vector3 endPoint = start + laserDirection * _laserDistance;

		if (Physics.Raycast(start, laserDirection, out RaycastHit hit, _laserDistance))
		{
			endPoint = hit.point;
		}

		_aimLaser.SetPosition(0, start);
		_aimLaser.SetPosition(1, Vector3.Lerp(start, endPoint, 0.2f));
		_aimLaser.SetPosition(2, Vector3.Lerp(start, endPoint, 0.7f));
		_aimLaser.SetPosition(3, endPoint);
	}

	private void HandleGrapStatusChange(bool isWeaponGrab)
	{
		_aimLaser.enabled = isWeaponGrab;
	}

	private void HandleReloadStatusChange(bool isReload)
	{
		_aimLaser.enabled = !isReload;
	}
}
