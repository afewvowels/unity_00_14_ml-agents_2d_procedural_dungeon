using UnityEngine;

public class Item : MonoBehaviour
{
    public int restoreAmount;
    public GameObject itemMesh;
    public enum ItemClass
    {
        HealthPotion,
        ManaPotion,
        StaminaPotion,
        Key
    }
}