using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour, IInteractable
{
	public MeshRenderer MeshRenderer { get; private set; }
	public Vector3 Position => transform.position;
	public GameObject GameObject => gameObject;

	[SerializeField] private AmmoPackDataSO _packData;
	[SerializeField] [ColorUsage(true, true)] private Color _normalColor, _highlightColor;

	private readonly int _emissionColorHash = Shader.PropertyToID("_EmissionColor");

	public void SetUpAmmoPackData(AmmoPackDataSO data)
	{
		_packData = data;
		SetPickUpModel();
	}

	private void SetPickUpModel()
	{
		Transform trm = transform.Find($"Ammo_{_packData.boxType}");
		MeshRenderer = trm.GetComponent<MeshRenderer>();
		trm.gameObject.SetActive(true);
	}

	private void Start()
	{
		if(MeshRenderer == null) SetPickUpModel();
	}

	public void InteractWith(Player player)
	{
		player.GetCompo<PlayerWeaponController>().PickUpAmmoPack(_packData);
		Destroy(GameObject);
	}

	public void SetHighlight(bool isHighlight)
	{
		Color targetColor = isHighlight ? _highlightColor : _normalColor;
		MeshRenderer.material.SetColor(_emissionColorHash, targetColor);
	}
}
