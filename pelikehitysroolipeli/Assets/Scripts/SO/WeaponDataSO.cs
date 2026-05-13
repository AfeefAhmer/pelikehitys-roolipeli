using UnityEngine;

public enum WeaponType { Melee, Ranged, Magic }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public WeaponType weaponType;
    public string specialAbility;

    [Header("Stats")]
    public int damage;
    public float range;

    [Header("Visual")]
    public Sprite sprite;
    public Sprite icon;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
}