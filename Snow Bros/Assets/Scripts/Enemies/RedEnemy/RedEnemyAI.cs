using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyAI : MonoBehaviour {

    private CircleCollider2D redEnemyColider;
    private Animator redEnemyAnimator;
    public  Rigidbody2D redEnemyBody;
    private Transform playerTranform;
    public PhysicsMaterial2D bounce;

    public float walkingDistance = 10.0f;
    public bool isJumpPosition = false;

    public bool grounded = false;
    public bool walled = false;
    private bool playerkicked = false; //nếu bị player đá thì sẽ k còn tan băng
    public float jumpForce = 1400f;

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
   
    //Enemy Information
    public int Health = 100;

    private void Awake()
    {
        redEnemyColider = GetComponent<CircleCollider2D>();
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
        if (time > 1.0f && !playerkicked)
        {
            Health = Mathf.Min(100, Health + 4);
            time -= 1.0f;
        }
        if (Health < 99) Animation_Freeze();
        else
        {
            if (gameObject.layer != 14)
            {
                gameObject.tag = "Enemy";
                gameObject.layer = 9;
            }
        }


       // transform.LookAt(target);
       if (redEnemyAnimator.GetInteger("RedEnemyCurrentState")==STATE_WALK 
            && (
            (redEnemyBody.position.x< playerTranform.position.x && transform.localScale.x<0)
            ||(redEnemyBody.position.x > playerTranform.position.x && transform.localScale.x > 0)
            )
            )
        {
            if (Mathf.Abs(redEnemyBody.position.y - playerTranform.position.y) < 1.0f)
                redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_TURN);
             
        }
        if (isJumpPosition==true && (redEnemyAnimator.GetInteger("RedEnemyCurrentState")==STATE_WALK)
            &&playerTranform.position.y>transform.position.y+0.5f)
        {
            
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_JUMP);
            isJumpPosition = false;
            Debug.Log("Jump");
           
        }

    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "JumpPosition")
            isJumpPosition = true;
        if (gameObject.layer == 14)
        {
            Debug.Log(redEnemyBody.velocity.y);
            if (
                ///redEnemyAnimator.GetInteger("RedEnemyCurrentState") == STATE_DIE1
              
                target.gameObject.tag == "Ground")
            {
                //if (transform.position.y>target.transform.position.y)
                redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_DIE2);
                
            }
        }
        else
        {
            if (target.gameObject.tag=="Freeze4")
            {
               
                gameObject.layer = 14;
                redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_DIE1);
                redEnemyBody.AddForce(new Vector2(Random.Range(-50.0f, 50.0f), Random.Range(400.0f, 600.0f)));
            }
            else
            {
                if (gameObject.tag == "Freeze4")
                {
                    //xử lý sau khi freeze4 rớt nhanh xuống thì set lại như cũ
                    if (target.gameObject.tag == "Ground")
                    {
                        redEnemyBody.velocity = new Vector2(redEnemyBody.velocity.x, 0);
                        redEnemyBody.gravityScale = 1;
                    }
                    else if (target.gameObject.tag == "Wall")
                    {
                        redEnemyBody.velocity = Vector2.zero;
                        Destroy(gameObject, .5f);
                    }
                }
                else if (gameObject.tag == "Enemy")
                {
                    if ((target.gameObject.tag == "Ground") )
                    {
                        redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_IDLE);
                        grounded = true;
                    }
                    else
                        if (target.gameObject.tag == "Wall" && walled == false)
                    {
                        walled = true;
                        redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_TURN);
                    }
                }
            }
        }
        
    }

    private void OnCollisionStay2D(Collision2D target)
    {
        if (gameObject.tag == "Freeze4" && target.gameObject.tag == "Ground")
        {
            redEnemyBody.velocity = new Vector2(redEnemyBody.velocity.x, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D target)
    {
        if (target.gameObject.tag == "JumpPosition")
            isJumpPosition = false;
        if ((target.gameObject.tag == "Ground")&& redEnemyBody.velocity.y < -0.0f)
        {
            //xử lý freeze4 rớt nhanh xuống
            if(gameObject.tag == "Freeze4")
            {
                redEnemyBody.gravityScale = 100;
            }
            else if (gameObject.tag == "Enemy")
            {
                grounded = false;
                if (gameObject.layer!=14) redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FALL);
                //Debug.Log("RedEnemy Fall with velocity: " + redEnemyBody.velocity.y);
            }            
        }
        else if (target.gameObject.tag=="Wall")
        {
            walled = false;
        }
    }

    void PlayerKicked(int direction)
    {
        playerkicked = true;
        redEnemyBody.AddForce(new Vector2(3000f * direction, 0));
        redEnemyBody.sharedMaterial = bounce;
        redEnemyBody.freezeRotation = false;
    }

    void Damage(int dmg)
    {
        if (gameObject.layer!=14)
        Health = Mathf.Max(0, Health - dmg);
    }

    void Animation_Freeze()
    { 
        if (gameObject.layer==14) return;
        if (Health <= 25)
        {
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE4);
            gameObject.tag = "Freeze4";
            gameObject.layer = 11;
            redEnemyBody.mass = 2;
        }
        else if (Health <= 45)
        {
            gameObject.layer = 12;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE3);
            gameObject.tag = "Freeze";
            redEnemyBody.mass = 1;
        }
        else if (Health <= 75)
        {
            gameObject.layer = 12;
            redEnemyAnimator.SetInteger("RedEnemyCurrentState", STATE_FREEZE2);
            gameObject.tag = "Freeze";
        }
        else if (Health < 100)
        {
            gameObject.layer = 12;
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
