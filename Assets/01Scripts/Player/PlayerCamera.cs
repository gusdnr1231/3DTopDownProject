using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField] private GameEventChannelSO _valueChannelSO;
	[SerializeField] private float _distanceChangeRate = 2f, _distanceThreshold = 0.25f;
	private bool _isChangeComplete;

	private CinemachineVirtualCamera _followCam;
	private CinemachineFramingTransposer _framingTransposer;

	private float _targetCamDistance;

	private void Awake()
	{
		_followCam = GetComponent<CinemachineVirtualCamera>();
		_framingTransposer = _followCam.GetCinemachineComponent<CinemachineFramingTransposer>();
		_valueChannelSO.AddListener<CamDistance>(HandleCamDistanceChange);

		_targetCamDistance = 5f;
	}
	
	private void HandleCamDistanceChange(CamDistance evt)
	{
		_targetCamDistance = evt.distance;
		_isChangeComplete = false;
	}

	private void Update()
	{
		UpdateCameraDistance();
	}

	private void UpdateCameraDistance()
	{
		if (_isChangeComplete) return;
		float currentDistance = _framingTransposer.m_CameraDistance;
		if (Mathf.Abs(currentDistance - _targetCamDistance) < _distanceThreshold)
		{
			_framingTransposer.m_CameraDistance = _targetCamDistance;
			_isChangeComplete = true;
		}
		else
		{
			_framingTransposer.m_CameraDistance
				= Mathf.Lerp(_framingTransposer.m_CameraDistance, _targetCamDistance, _distanceThreshold * Time.deltaTime);
		}
	}

}
