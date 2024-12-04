using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyVisual : MonoBehaviour, IEnemyCompomnent
{
	[Header("Crystal Visual")]
	[SerializeField] private EnemyCrystal[] _crystals;
	[SerializeField] private int _crystalAmount;

	[Header("Color")]
	[SerializeField] private Material[] _colorMaterials;
	[SerializeField] private SkinnedMeshRenderer _meshRenderer;

	private EnemyBase _enemy;

	public void Initilize(EnemyBase enemy)
	{
		_enemy = enemy;

		_crystals = GetComponentsInChildren<EnemyCrystal>();
	}

	public void AfterInitilize()
	{
		SetUpRandomCrystal();
		SetUpRandomColor();
	}

	private void SetUpRandomColor()
	{
		int idx = Random.Range(0, _colorMaterials.Length);
		_meshRenderer.material = _colorMaterials[idx];
	}

	private void SetUpRandomCrystal()
	{
		//_crystal.ToList().ForEach(x => x.SetActiveCrystal(false));

		for(int i = 0; i < _crystalAmount; i++)
		{
			int randIdx = Random.Range(0, _crystals.Length - i);
			int lastIdx = _crystals.Length - i - 1;

			(_crystals[randIdx], _crystals[lastIdx]) = (_crystals[lastIdx], _crystals[randIdx]);
		}

		for(int i = 0; i < _crystals.Length - _crystalAmount; i++)
		{
			_crystals[i].SetActiveCrystal(false);
		}
	}

}
