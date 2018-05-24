using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyAI : MonoBehaviour {

    private Animator redEnemyAnimator;
    public  Rigidbody2D redEnemyBody;
    private Transform playerTranform;

    public bool grounded = false;
    public bool walled = false;

    public int STATE_FALL = -2;
    public int STATE_LAND = -1;
    public int STATE_IDLE = 0;
    public int STATE_TURN = 2;
    public int STATE_WALK = 1;
    public int STATE_FREEZE1 = 3;
    public int STATE_FREEZE2 = 4;
    public int STATE_FREEZE3 = 5;
    public int STATE_FREEZE4 = 6;
    public int STATE_DIE1 = 8;
    public int STATE_DIE2 = 9;
    public int STATE_WAKEUP = 7;
    public int STATE_JUMP = 10;
    public float moveXVelocity = 100f;
    public float moveForce = 150f;
    public float maxVelocity = 1.5f;
    public float time = 0.0f;
   
    //Enemy Infomation
    public int Health = 100;

    private void Awake()
    {
        redEnemyBody = GetComponent<Rigidbody2D>();
        redEnemyAnimator = GetComponent<Animator>();
        playerTranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1.0f)
        {
            Health = Mathf.Min(100, Health + 4);
            time -= 1.0f;
        }
        if (Health<99) Animation_Freeze();
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if ((target.gameObject.tag == "Ground") && redEnemyBody.velocity.y < 0.1f)
        {

            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_IDLE);
            grounded = true;
        }
        else
            if (target.gameObject.tag == "Wall"&&walled==false)
        {
            walled = true;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_TURN);
        }
        if (target.gameObject.tag == "Freeze4")
        {
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_DIE1);
        }
    }
    private void OnCollisionExit2D(Collision2D target)
    {
        if ((target.gameObject.tag == "Ground")&& redEnemyBody.velocity.y < -0.15f)
        {

            grounded = false;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FALL);
            Debug.Log("RedEnemy Fall with velocity: " +redEnemyBody.velocity.y);
        }
        else if (target.gameObject.tag=="Wall")
        {
            walled = false;
        }
    }

    void Damage(int dmg)
    {
        Health = Mathf.Max(0, Health - dmg);
    }

    void Animation_Freeze()
    {
        if (Health <= 25)
        {
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE4);
            gameObject.tag = "Freeze4";
            gameObject.layer = 11;
        }
        else if (Health <= 45)
        {
            gameObject.layer = 9;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE3);
            gameObject.tag = "Freeze";
        }
        else if (Health <= 75)
        {
            gameObject.layer = 9;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE2);
            gameObject.tag = "Freeze";
        }
        else if (Health < 100)
        {
            gameObject.layer = 9;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE1);
            gameObject.tag = "Freeze";
        }
        /*else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 9;
            if (redEnemyAnimator.GetInteger("RedEnemyCurrentState") == STATE_FREEZE1)
                redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_IDLE);
        }*/

    }





}
