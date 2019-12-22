using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAgent : Agent
{
    [SerializeField]
    [Range(8.0f, 20.0f)]
    public float moveSpeed;
    public DungeonManager dungeonManager;
    private int health;
    private int maxHealth;

    private int mana;
    private int maxMana;

    private int stamina;
    private int maxStamina;

    private Weapon[] weapons;
    private int[] activeWeapon;
    private int maxWeapons;

    private Item[] items;
    private int[] activeItem;
    private int maxItems;

    private int score;

    private bool isAttacking;

    private enum Facing
    {
        North,
        South,
        East,
        West
    }
    private Facing currentDirection;
    private Animator animator;
    private Animation agentAnimation;
    private Rigidbody2D rb2d;
    private BoxCollider2D agentCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agentAnimation = GetComponent<Animation>();
        rb2d = GetComponent<Rigidbody2D>();
        agentCollider = GetComponent<BoxCollider2D>();

        ResetStats();
    }

    public override void CollectObservations()
    {

    }

    public override void AgentAction(float[] vectorAction)
    {
        var moveUpDownAction = Mathf.FloorToInt(vectorAction[0]);
        var moveLeftRightAction = Mathf.FloorToInt(vectorAction[1]);
        var attackAction = Mathf.FloorToInt(vectorAction[2]);
        var itemAction = Mathf.FloorToInt(vectorAction[3]);

        switch(moveUpDownAction)
        {
            case 1:
                rb2d.AddForce(transform.up * moveSpeed, ForceMode2D.Force);
                break;
            case 2:
                rb2d.AddForce(transform.up * -moveSpeed, ForceMode2D.Force);
                break;
        }

        switch(moveLeftRightAction)
        {
            case 1:
                rb2d.AddForce(transform.right * -moveSpeed, ForceMode2D.Force);
                break;
            case 2:
                rb2d.AddForce(transform.right * moveSpeed, ForceMode2D.Force);
                break;
        }

        switch (attackAction)
        {
            case 1:
                if (!isAttacking)
                {
                    StartCoroutine(DoAttack());
                }
                break;
            case 2:
                break;
        }

        switch (itemAction)
        {
            case 1:
                break;
            case 2:
                break;
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[4];

        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            action[1] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            action[1] = 2.0f;
        }

        return action;
    }

    public override void AgentReset()
    {
        ResetStats();
        dungeonManager.IncrementRoomCount();
        dungeonManager.CreateDungeon();
    }

    private void ResetStats()
    {
        rb2d.velocity = Vector2.zero;

        isAttacking = false;

        health = maxHealth;
        mana = maxMana;
        stamina = maxStamina;

        maxWeapons = 4;
        weapons = new Weapon[maxWeapons];
        activeWeapon = new int[maxWeapons];

        for (int i = 0; i < maxWeapons; i++)
        {
            weapons[i] = null;
            activeWeapon[i] = 0;
        }

        maxItems = 4;
        items = new Item[maxItems];
        activeItem = new int[maxItems];

        for (int i = 0; i < maxItems; i++)
        {
            items[i] = null;
            activeItem[i] = 0;
        }

        score = 0;
    }

    public void UpdateScore(int inScore)
    {
        score += inScore;
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.1f);

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log(c.gameObject.tag.ToString());
        if (c.gameObject.CompareTag("goal"))
        {
            AddReward(1.0f);
            Done();
        }
    }
}