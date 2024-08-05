using UnityEngine;

[System.Serializable] public class WeaponInfo {
    public string weaponName;
    public WeaponType weaponType;
    public GameObject weaponObject;
}

public enum WeaponType {
    CombatWeapon,
    Gun,
    Explosive
}