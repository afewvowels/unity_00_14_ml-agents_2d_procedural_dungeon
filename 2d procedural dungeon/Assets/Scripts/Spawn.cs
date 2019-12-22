using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public enum SpawnClass
    {
        Agent,
        Goal,
        Boss,
        Enemy,
        Item,
        Key
    }

    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float spawnChance;

    public SpawnClass spawnClass;

    public void DoSpawn(GameObject spawnable, Transform parentTransform)
    {
        if (Random.value < spawnChance)
        {
            GameObject spawned = (GameObject)Instantiate(spawnable);
            spawned.transform.position = transform.position;
            spawned.transform.SetParent(parentTransform, false);
        }
    }
}