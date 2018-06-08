using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyBoss1 : MonoBehaviour {
    
    private Animator enemyBoss1Animator;
    public Rigidbody2D enemyBoss1Body;
    public PhysicsMaterial2D bounce;
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
    public int STATE_WAKEUP = 7;

    public float moveXVelocity = 100f;
    public float moveForce = 150f;
    public float maxVelocity = 1.5f;
    public float time=0.0f;

    //Enemy Infomation
    public int Health=100;
    bool playerkicked = false;
    // Use this for initialization
    void Start()
    {
        enemyBoss1Body.AddForce(new Vector2(-Random.Range(500.0f,1000.0f), Random.Range(400.0f,800.0f)));
       
    }
    void Awake()
    {
        enemyBoss1Animator = GetComponent<Animator>();
        enemyBoss1Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        
        if (time > 1.0f&&!playerkicked)
        {
            Health = Mathf.Min(100, Health + 4);   
            time -= 1.0f;
        }
        if (Health<100) Animation_Freeze();
        else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 9;
        }
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if ((target.gameObject.tag == "Ground" ) && enemyBoss1Body.velocity.y < 0.1f)
        {
            
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_LAND);         
            grounded = true;
        }
        
            if (target.gameObject.tag == "Wall"&&walled==false)
        {
            walled = true;
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_TURN);
        }
        if (target.gameObject.tag == "Freeze4"&&Mathf.Abs( target.rigidbody.velocity.x)>2.0f)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionExit2D(Collision2D target)
    {
        if ((target.gameObject.tag == "Ground")&& enemyBoss1Body.velocity.y < 0.1f)
        {
            if (gameObject.tag != "Freeze" && gameObject.tag != "Freeze4")
            {
                grounded = false;
                enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_FALL);
              //  Debug.Log("EnemyBoss1 Fall with velocity: " + enemyBoss1Body.velocity.y);
            }
        }
        else
            if (target.gameObject.tag == "Wall" )
        {
            walled = false;
        }
    }

    void Damage(int dmg)
    {
        Health = Mathf.Max(0,Health-dmg);
    }
    void PlayerKicked(int direction)
    {
        playerkicked = true;
        enemyBoss1Body.AddForce(new Vector2(3000f * direction, 0));
        enemyBoss1Body.sharedMaterial = bounce;
        enemyBoss1Body.freezeRotation = false;
    }
    void Animation_Freeze()
    {
        if (Health <= 25)
        {
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_FREEZE4);
            gameObject.tag = "Freeze4";
            gameObject.layer = 11;
        }
        else if (Health <= 45)
        {
            gameObject.layer = 12;
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_FREEZE3);
            gameObject.tag = "Freeze";
        }
        else if (Health <= 75)
        {
            gameObject.layer = 12;
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_FREEZE2);
            gameObject.tag = "Freeze";
        }
        else if (Health <100)
        {
            gameObject.layer = 12;
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_FREEZE1);
            gameObject.tag = "Freeze";
        }
       /* else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 9;
            if (enemyBoss1Animator.GetInteger("EnemyBoss1CurrentState")== STATE_FREEZE1)
            enemyBoss1Animator.SetInteger("EnemyBoss1CurrentState", STATE_WAKEUP);
        }*/

    }

}
