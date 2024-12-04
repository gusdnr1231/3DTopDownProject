using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private float _walkSpeed, _runSpeed, _gravity = -9.8f, _rotationSpeed;

    private CharacterController _characterController;
    
    public bool IsGround => _characterController.isGrounded;
    public bool IsRunning {get; private set;}

    public event Action<Vector3> OnMovement;
    public event Action<bool> OnRunning;

    private InputReaderSO _inputCompo;

    private Player _player;
    private Vector3 _movement;
    private float _verticalVelocity;
    private Quaternion _targetRotation;

    public void Initialize(Player player)  //여기서 컴포넌트를 가져온다는거.
    {
        _player = player;
        _characterController = GetComponent<CharacterController>();

        _inputCompo = _player.GetCompo<InputReaderSO>();
        _player.GetCompo<PlayerAim>().OnLookDirectionChange += HandleLookChange;

        _inputCompo.RunEvent += HandleRunEvent;
    }

    private void HandleLookChange(Quaternion rotation)
    {
        _targetRotation = rotation;
    }

    private void HandleRunEvent(bool isRun)
    {
        if(isRun != IsRunning)
        {
            OnRunning?.Invoke(isRun);
        }

        IsRunning = isRun;
    }

    private void FixedUpdate() {
        CalculateMovement();
        ApplyGravity();
        ApplyRotation();
        CharacterMove();
    }

    private void CalculateMovement()
    {
        Vector3 moveInput = _inputCompo.Movement;

        _movement = new Vector3(moveInput.x, 0, moveInput.y);
        OnMovement?.Invoke(_movement);

        float speed = IsRunning ? _runSpeed : _walkSpeed;
        _movement *= speed * Time.fixedDeltaTime;
    }

    private void ApplyGravity()
    {
        if(IsGround && _verticalVelocity < 0)
            _verticalVelocity = -2f;
        else 
            _verticalVelocity += _gravity * Time.fixedDeltaTime;

        _movement.y = _verticalVelocity;
    }

    private void ApplyRotation()
    {
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, _targetRotation, Time.fixedDeltaTime * _rotationSpeed);
    }

    private void CharacterMove()
    {
        _characterController.Move(_movement);
    }

}
