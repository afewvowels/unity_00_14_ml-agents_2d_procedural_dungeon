using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public GameObject weaponMesh;
    public enum WeaponClass
    {
        Dagger,
        Sword,
        Axe,
        Staff,
        Bow
    }
    public WeaponClass weaponClass;
}