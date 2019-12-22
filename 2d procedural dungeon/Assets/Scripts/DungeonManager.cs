using System.Collections;
using UnityEngine;
using UniRx.Async;

public class DungeonManager : MonoBehaviour
{
    public Camera mainCamera;
    public DungeonAgent agent;
    public GameObject instantiatedContainer;
    public GameObject dungeonGeneratorGO;
    public int roomCount;
    public Assets.ProceduralLevelGenerator.Examples.ProceduralLevelGraphs.Scripts.ProceduralInputConfig mapConfigParameters;
    private Assets.ProceduralLevelGenerator.Scripts.GeneratorPipeline.DungeonGenerators.DungeonGeneratorPipeline dungeonGenerator;

    private void Start()
    {
        roomCount = 2;
        dungeonGenerator = dungeonGeneratorGO.GetComponent<Assets.ProceduralLevelGenerator.Scripts.GeneratorPipeline.DungeonGenerators.DungeonGeneratorPipeline>();
    }

    private void ParentCamera(Transform parentTransform = null)
    {
        mainCamera.transform.SetParent(parentTransform);
    }

    public void IncrementRoomCount()
    {
        roomCount++;
    }

    public void ResetRoomCount()
    {
        roomCount = 2;
    }

    async public UniTask CreateDungeon()
    {
        bool isGenerated = false;
        // ParentCamera();
        agent.GetComponent<Rigidbody2D>().isKinematic = true;

        if (instantiatedContainer.transform.childCount > 0)
        {
            await DestroyDungeon();
        }

        mapConfigParameters.MinLength = roomCount;
        mapConfigParameters.MaxLength = (roomCount * 2) - (roomCount / 2);

        if (roomCount > 5)
        {
            mapConfigParameters.AddRedundantRooms = true;
        }
        else
        {
            mapConfigParameters.AddRedundantRooms = false;
        }

        if (roomCount > 8)
        {
            mapConfigParameters.AddShortcuts = true;
        }
        else
        {
            mapConfigParameters.AddShortcuts = false;
        }

        while (!isGenerated)
        {
            try
            {
                dungeonGenerator.Generate();
                GameObject dRoot = dungeonGenerator.PayloadInitializer.GetDungeonRoot();
                dRoot.transform.SetParent(instantiatedContainer.transform, false);
                await PullUpSpawns();
                isGenerated = true;
            }
            catch
            {
                DestroyOrphanedDungeon();
                StopAllCoroutines();
                StartCoroutine(CheckIfCreated());
                isGenerated = false;
            }
        }
    }

    async public UniTask DestroyDungeon()
    {
        for (int i = 0; i < instantiatedContainer.transform.childCount; i++)
        {
            Destroy(instantiatedContainer.transform.GetChild(i).gameObject); 
            await UniTask.DelayFrame(1);
        }
    }
    
    public void DestroyOrphanedDungeon()
    {
        foreach (GameObject dungeonTilemapRoot in GameObject.FindGameObjectsWithTag("dungeontilemaproot"))
        {
            if (dungeonTilemapRoot.transform.parent == null)
            {
                Destroy(dungeonTilemapRoot);
                break;
            }
        }
    }

    private IEnumerator CheckIfCreated()
    {
        yield return new WaitForSeconds(7.0f);
        if (instantiatedContainer.transform.childCount == 0)
        {
            CreateDungeon();
        }
    }

    async private UniTask PullUpSpawns()
    {
        Transform toTransform = instantiatedContainer.transform;
        Transform roomsTransform = toTransform.GetChild(0).GetChild(6);
        int generatedRoomsCount = roomsTransform.childCount;

        for (int i = 0; i < generatedRoomsCount; i++)
        {
            for (int j = 0; j < roomsTransform.GetChild(i).childCount; j++)
            {
                GameObject roomObject = roomsTransform.GetChild(i).GetChild(j).gameObject;
                if (roomObject.tag == "spawn")
                {
                    roomObject.transform.SetParent(toTransform, true);
                }
                await UniTask.DelayFrame(1);
            }
        }

        await DoSpawns();
    }

    async private UniTask DoSpawns()
    {
        Transform containerTransform = instantiatedContainer.transform;
        int count = containerTransform.childCount;

        for (int i = 0; i < count; i++)
        {
            GameObject child = containerTransform.GetChild(i).gameObject;
            if (child.tag == "spawn")
            {
                Spawn(child);
            }
            await UniTask.DelayFrame(1);
        }
    }

    private void Spawn(GameObject spawnPoint)
    {
        Spawn spawn = spawnPoint.GetComponent<Spawn>();

        switch ((int)spawn.spawnClass)
        {
            case 0:
                SpawnAgent(spawnPoint);
                break;
            case 1:
                SpawnGoal(spawnPoint);
                break;
            case 2:
                SpawnBoss(spawnPoint);
                break;
            case 3:
                SpawnEnemy(spawnPoint);
                break;
            case 4:
                SpawnItem(spawnPoint);
                break;
            case 5:
                SpawnKey(spawnPoint);
                break;
        }
    }

    private void SpawnAgent(GameObject spawnPoint)
    {
        agent.transform.position = spawnPoint.transform.position;
        // ParentCamera(agent.transform);
        // mainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
        agent.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void SpawnGoal(GameObject spawnPoint)
    {
        spawnPoint.gameObject.tag = "goal";
    }

    private void SpawnBoss(GameObject spawnPoint)
    {
        
    }

    private void SpawnEnemy(GameObject spawnPoint)
    {

    }

    private void SpawnItem(GameObject spawnPoint)
    {

    }

    private void SpawnKey(GameObject spawnPoint)
    {

    }
}