using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IInteractable
{
	[field:SerializeField] public WeaponDataSO WeaponData { get; private set; }
	public MeshRenderer MeshRenderer { get; private set; }
	public Vector3 Position => transform.position;
	public GameObject GameObject => gameObject;

	[SerializeField] [ColorUsage(true, true)] private Color _normalColor, _highlightColor;
	private readonly int _emissionHash = Shader.PropertyToID("_EmissionColor");

	public int ReservedAmmo { get; private set; }
	public int AmmoInMagazine { get; private set; }

	private void Start()
	{
		if(MeshRenderer == null)
		{
			SetWeaponData();
			AmmoInMagazine = WeaponData.maxAmmo;
			ReservedAmmo = 0;
		}
	}

	public void SetUpWeaponData(PlayerWeapon dropWeapon)
	{
		WeaponData = dropWeapon.weaponData;
		AmmoInMagazine = dropWeapon.bulletInMagazine;
		ReservedAmmo = dropWeapon.reservedAmmo;

		SetWeaponData();
	}

	private void SetWeaponData()
	{
		var trm = transform.Find(WeaponData.gunTrmName);
		trm.gameObject.SetActive(true);
		MeshRenderer = trm.GetComponent<MeshRenderer>();
	}

	public void InteractWith(Player player)
	{
		player.GetCompo<PlayerWeaponController>().PickUpWeapon(this);
		Destroy(gameObject);
	}

	public void SetHighlight(bool isHighlight)
	{
		Color targetColor = isHighlight ? _highlightColor : _normalColor;
		MeshRenderer.material.SetColor(_emissionHash, targetColor);
	}

	private void OnValidate()
	{
		if(WeaponData != null)
		{
			gameObject.name = $"PickUpWeapon - {WeaponData.gunTrmName}";
		}
	}
}
