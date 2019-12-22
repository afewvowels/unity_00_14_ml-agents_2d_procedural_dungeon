using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private List<GameObject> dungeons;
    private int activeIndex;
    private DungeonAgent activeAgent;
    private DungeonRoot activeDungeonRoot;
    private bool isParentedToAgent;

    private void Start()
    {
        dungeons = new List<GameObject>();
        activeIndex = 0;
        foreach (GameObject dungeon in GameObject.FindGameObjectsWithTag("dungeonroot"))
        {
            dungeons.Add(dungeon);
        }
    }

    private void Update()
    {
        float multiplier = 1.0f;

        if (Input.GetKey(KeyCode.LeftShift) && !isParentedToAgent)
        {
            multiplier *= 5.0f;
        }

        if (Input.GetKey(KeyCode.UpArrow) && !isParentedToAgent)
        {
            MoveUp(multiplier);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isParentedToAgent)
        {
            MoveDown(multiplier);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !isParentedToAgent)
        {
            MoveLeft(multiplier);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isParentedToAgent)
        {
            MoveRight(multiplier);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<Camera>().orthographicSize++;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<Camera>().orthographicSize--;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isParentedToAgent)
            {
                ParentToNull();
            }
            else
            {
                ParentToActiveAgent();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeActiveDungeon();
        }
    }

    private void ChangeActiveDungeon()
    {
        ParentToNull();
        activeIndex++;
        if (activeIndex >= dungeons.Count)
        {
            activeIndex = 0;
        }
        activeDungeonRoot = dungeons[activeIndex].GetComponent<DungeonRoot>();
        activeAgent = activeDungeonRoot.dungeonAgent;
        ParentToActiveAgent();
    }

    private void ParentToActiveAgent()
    {
        transform.SetParent(activeAgent.transform, false);
        transform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
        GetComponent<Camera>().orthographicSize = 5;
    }

    private void ParentToNull()
    {
        transform.SetParent(null);
    }

    public void DoScroll()
    {

    }

    public void MoveUp(float multiplier)
    {
        transform.position += Vector3.up * multiplier;
    }

    public void MoveDown(float multiplier)
    {
        transform.position -= Vector3.up * multiplier;
    }

    public void MoveLeft(float multiplier)
    {
        transform.position -= Vector3.right * multiplier;
    }

    public void MoveRight(float multiplier)
    {
        transform.position += Vector3.right * multiplier;
    }
}