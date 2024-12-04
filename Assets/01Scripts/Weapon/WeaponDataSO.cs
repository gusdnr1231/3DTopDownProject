using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrabType
{
    Side = 0, Back = 1
}

public enum ShootType
{
    Single = 0, Auto = 1
}

[CreateAssetMenu(menuName = "SO/Weapon/Data")]
public class WeaponDataSO : ScriptableObject
{
    public string gunTrmName;

    [Header("Weapon Info")]
    public float recoilPower;
    public float reloadSpeed;
    public float equipSpeed;
    [Space]
    public ShootType shootType;
    public float fireRate = 1;
    public float camDistance = 6;

    [Header("Bullet Spread")]
    public float spreadAmount;
    public float maxSpreadAmount;
    public float spreadIncRate = 0.15f;
    public float spreadCooldown = 1f;

    [Header("Ammo Data")]
    public int maxAmmo;
    public float shootingRange = 2f;
    public float bulletSpeed = 60f;
    public int bulletPerShot = 1;
    public float impactForce;
    public float damage = 2;

    [Header("Animation Setup")]
    public int animationLayer;
    public GrabType grabType;

    [Header("Left Hand IK")]
    public Vector3 leftHandPosition;
    public Vector3 leftHandRotation;
    public Vector3 leftHandHintPosition;
}
