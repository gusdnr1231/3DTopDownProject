using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour, IEnemyCompomnent
{
	[Header("Weapon Model")]
	[SerializeField] private Transform _weaponHolder;
	[SerializeField] private MeleeWeaponModel[] _weaponModels;
	[SerializeField] private MeleeWeaponType _weaponType;

	public MeleeWeaponModel currentWeapon;

	[Header("Attack Data")]
	public MeleeAttackDataSO attackData;
	public List<MeleeAttackDataSO> atkList;
	[SerializeField] private float _closedDistance = 1.3f;

	[Header("Add On Refernce")]
	[SerializeField] private EnemyShiled _shiled;

	private EnemyMelee _enemy;

	public void Initilize(EnemyBase enemy)
	{
		_enemy = enemy as EnemyMelee;

		_weaponModels = _weaponHolder.GetComponentsInChildren<MeleeWeaponModel>(true);
	}

	public void AfterInitilize()
	{
		ApplyAddOn();
		SetUpRandomWeapon();
	}

	private void ApplyAddOn()
	{
		switch (_enemy.addOnType)
		{
			case MeleeAddOn.Shiled:
				_weaponType = MeleeWeaponType.OneHand;
				_enemy.GetCompo<EnemyAnimator>().SetChaseIndex(1);
				_shiled.gameObject.SetActive(true);
				_shiled.OnDestroyShiled += () => _enemy.GetCompo<EnemyAnimator>().SetChaseIndex(0);
				_enemy.GetCompo<EnemyHealth>().OnDead += () =>
				{
					_shiled.DropShiled();
				};
				break;
			case MeleeAddOn.Dodge:
				_weaponType = MeleeWeaponType.UnArmed;
				break;
		}
	}

	private void SetUpRandomWeapon()
	{
		List<MeleeWeaponModel> filterList = new List<MeleeWeaponModel>();

		foreach(var model in _weaponModels)
		{
			model.SetActiveModel(false);
			if(model.weaponType == _weaponType) filterList.Add(model);
		}

		if(filterList.Count <= 0)
		{
			Debug.Log($"No Weapon - {gameObject.name}");
			return;
		}

		int idx = Random.Range(0, filterList.Count);

		currentWeapon = filterList[idx];
		currentWeapon.SetActiveModel(true);

		_enemy.GetCompo<EnemyAnimator>().SetAnimatorControllerIfExist(currentWeapon.animController);
		atkList = currentWeapon.atkDataList;
		attackData = atkList[0];
	}

	public bool IsPlayerInAttackRange()
	{
		Vector3 targetPos = _enemy.TargetTrm.position;
		return Vector3.Distance(targetPos, transform.position) < attackData.atkRange;
	}

	public bool IsPlayerClosed()
	{
		Vector3 targetPos = _enemy.TargetTrm.position;
		return Vector3.Distance(targetPos, transform.position) < _closedDistance;
	}

	public void UpdateNextAttack()
	{
		List<MeleeAttackDataSO> vaildAttacks;

		var type = IsPlayerClosed() ? MeleeAttackType.Close : MeleeAttackType.Charge;
		vaildAttacks = atkList.Where(x => x.atkType == type).ToList();

		int randIdx = Random.Range(0, vaildAttacks.Count);
		attackData = vaildAttacks[randIdx];
	}

}
