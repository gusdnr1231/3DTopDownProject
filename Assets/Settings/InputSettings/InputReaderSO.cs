using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions, IPlayerComponent
{
	public Vector2 Movement { get; private set; }
	public Vector2 MousePosition { get; private set; }

	[SerializeField] private LayerMask _whatIsGround, _whatIsEnemy;
	private Vector3 _beforeMouseWorldPosition;
	
	public event Action<bool> RunEvent;
	public event Action<bool> FireEvent;
	public event Action<int> ChangeWeaponSlotEvent;
	public event Action ReloadEvent;
	public event Action InteractionEvent;

	private Controls _controls;
	private Player _player;

	private void OnEnable()
	{
		if (_controls == null)
		{
			_controls = new Controls();
			_controls.Enable();
			_controls.Player.SetCallbacks(this);
		}
	}

	public void OnMovement(InputAction.CallbackContext context)
	{
		Movement = context.ReadValue<Vector2>();
	}

	public void OnAim(InputAction.CallbackContext context)
	{
		MousePosition = context.ReadValue<Vector2>();
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			RunEvent?.Invoke(true);
		}
		else if (context.canceled)
		{
			RunEvent?.Invoke(false);
		}
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			FireEvent?.Invoke(true);
		}
		else if (context.canceled)
		{
			FireEvent?.Invoke(false);
		}
	}

	public Vector3 GetWorldMousePosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(MousePosition);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsGround))
		{
			_beforeMouseWorldPosition = hitInfo.point;
		}
		return _beforeMouseWorldPosition;
	}

	public RaycastHit GetMouseHitInfo()
	{
		Ray ray = Camera.main.ScreenPointToRay(MousePosition);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsEnemy))
		{
			return hitInfo;
		}
		return default;
	}

	public void Initialize(Player player)
	{
		_player = player;
	}

	public void OnEquipSlot1(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			ChangeWeaponSlotEvent?.Invoke(0);
		}
	}

	public void OnEquipSlot2(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			ChangeWeaponSlotEvent?.Invoke(1);
		}
	}

	public void OnEquipSlot3(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			ChangeWeaponSlotEvent?.Invoke(2);
		}
	}

	public void OnReload(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			ReloadEvent?.Invoke();
		}
	}

	public void OnInteraction(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			InteractionEvent?.Invoke();
		}
	}
}