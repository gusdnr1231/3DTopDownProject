using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IPlayerComponent
{
	private Player _player;
	private List<IInteractable> _interactionList;
	private IInteractable _closestOne;

	private void Update()
	{
		if(_interactionList.Count > 1)
		{
			SetHighlightClosestObject();
		}
	}

	public void Initialize(Player player)
	{
		_player = player;
		_interactionList = new List<IInteractable>();

		_player.GetCompo<InputReaderSO>().InteractionEvent += HandleInteraction;
	}

	private void HandleInteraction()
	{
		if(_closestOne == null) return;
		
		_closestOne.InteractWith(_player);
		_interactionList.Remove(_closestOne);
		_closestOne = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out IInteractable target))
		{
			_interactionList.Add(target);
			SetHighlightClosestObject();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IInteractable target))
		{
			_interactionList.Remove(target);
			target.SetHighlight(false);
			SetHighlightClosestObject();
		}
	}

	private void SetHighlightClosestObject()
	{
		Vector3 playerPos = transform.position;

		int nearIndex = -1;
		float minDistance = Mathf.Infinity;
		_closestOne = null;

		for(int i = 0; i < _interactionList.Count; i++)
		{
			float distance = Vector3.Distance(playerPos, _interactionList[i].Position);
			if(distance < minDistance)
			{
				minDistance = distance;
				nearIndex = i;
			}
			_interactionList[i].SetHighlight(false);
		}

		if(nearIndex >= 0)
		{
			_interactionList[nearIndex].SetHighlight(true);
			_closestOne = _interactionList[nearIndex];
		}
	}
}
