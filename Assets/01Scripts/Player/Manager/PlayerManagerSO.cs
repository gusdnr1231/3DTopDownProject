using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PlayerManager")]
public class PlayerManagerSO : ScriptableObject
{
    public Player Player { get; private set; }
    public Transform PlayerTrm { get; private set; }

    public event Action SetUpPlayerComplete;

    public void SetPlayer(Player player)
    {
        Player = player;
        PlayerTrm = player.transform;

        SetUpPlayerComplete?.Invoke();
    }
}
