using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyAI : MonoBehaviour
{

    private CircleCollider2D GreenEnemyColider;
    private Animator GreenEnemyAnimator;
    public Rigidbody2D GreenEnemyBody;
    private Transform playerTranform;
    public PhysicsMaterial2D bounce;

    public float startX, endX;
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
    public int STATE_ATTACK2 = 12;
    public float moveXVelocity = 100f;
    public float moveForce = 150f;
    public float maxVelocity = 1.5f;
    public float time = 0.0f;

    [SerializeField]
    private GameObject bulletFire;
    [SerializeField]
    private Transform shootPointHorizontal,shootPointVertical;
    [SerializeField]
    private Transform startPoint, endPoint;
    //Enemy Information
    public int Health = 100;
    public bool isShoot = false;

    private void Awake()
    {
        GreenEnemyColider = GetComponent<CircleCollider2D>();
        GreenEnemyBody = GetComponent<Rigidbody2D>();
        GreenEnemyAnimator = GetComponent<Animator>();
        playerTranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {
        startX = startPoint.position.x;
        endX = endPoint.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 1.0f && !playerkicked)
        {
            Health = Mathf.Min(100, Health + 10);
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


       
       if ((transform.position.x<startX&&transform.localScale.x<0.0f)||(transform.position.x>endX&& transform.localScale.x > 0.0f))
        {
            GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_TURN);
            GreenEnemyBody.velocity = new Vector2(0, 0);
        }

    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "JumpPosition")
            isJumpPosition = true;
        if (gameObject.layer == 14)
        {
            Debug.Log(GreenEnemyBody.velocity.y);
            if (
                ///GreenEnemyAnimator.GetInteger("GreenEnemyCurrentState") == STATE_DIE1

                target.gameObject.tag == "Ground")
            {
                //if (transform.position.y>target.transform.position.y)
                GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_DIE2);

            }
        }
        else
        {
            if (target.gameObject.tag == "Freeze4")
            {

                gameObject.layer = 14;
                GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_DIE1);
                GreenEnemyBody.AddForce(new Vector2(Random.Range(-50.0f, 50.0f), Random.Range(400.0f, 600.0f)));
            }
            else
            {
                if (gameObject.tag == "Freeze4")
                {
                    //xử lý sau khi freeze4 rớt nhanh xuống thì set lại như cũ
                    if (target.gameObject.tag == "Ground")
                    {
                        GreenEnemyBody.velocity = new Vector2(GreenEnemyBody.velocity.x, 0);
                        GreenEnemyBody.gravityScale = 1;
                    }
                    else if (target.gameObject.tag == "Wall")
                    {
                        GreenEnemyBody.velocity = Vector2.zero;
                        Destroy(gameObject, .5f);
                    }
                }
                else if (gameObject.tag == "Enemy")
                {
                    if ((target.gameObject.tag == "Ground"))
                    {
                        GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_IDLE);
                        grounded = true;
                    }
                    else
                        if (target.gameObject.tag == "Wall" && walled == false)
                    {
                        walled = true;
                        GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_TURN);
                    }
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D target)
    {
       // if (gameObject.tag == "Freeze4" && target.gameObject.tag == "Ground")
        {
           // GreenEnemyBody.velocity = new Vector2(GreenEnemyBody.velocity.x, 0);
        }
        if (gameObject.tag == "Freeze4" && target.gameObject.tag == "Boss")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D target)
    {
        if (target.gameObject.tag == "JumpPosition")
            isJumpPosition = false;
        if ((target.gameObject.tag == "Ground") && GreenEnemyBody.velocity.y < -0.0f)
        {
            //xử lý freeze4 rớt nhanh xuống
            if (gameObject.tag == "Freeze4")
            {
                GreenEnemyBody.gravityScale = 100;
            }
            else if (gameObject.tag == "Enemy")
            {
                grounded = false;
                if (gameObject.layer != 14) GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_FALL);
                //Debug.Log("GreenEnemy Fall with velocity: " + GreenEnemyBody.velocity.y);
            }
        }
        else if (target.gameObject.tag == "Wall")
        {
            walled = false;
        }
    }

    void PlayerKicked(int direction)
    {
        playerkicked = true;
        GreenEnemyBody.AddForce(new Vector2(3000f * direction, 0));
        GreenEnemyBody.sharedMaterial = bounce;
        GreenEnemyBody.freezeRotation = false;
    }

    void Damage(int dmg)
    {
        if (gameObject.layer != 14)
            Health = Mathf.Max(0, Health - dmg);
    }

    void Animation_Freeze()
    {
        if (gameObject.layer == 14) return;
        if (Health <= 25)
        {
            GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_FREEZE4);
            gameObject.tag = "Freeze4";
            gameObject.layer = 11;
            GreenEnemyBody.mass = 2;
        }
        else if (Health <= 45)
        {
            gameObject.layer = 12;
            GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_FREEZE3);
            gameObject.tag = "Freeze";
            GreenEnemyBody.mass = 1;
        }
        else if (Health <= 75)
        {
            gameObject.layer = 12;
            GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_FREEZE2);
            gameObject.tag = "Freeze";
        }
        else if (Health < 100)
        {
            gameObject.layer = 12;
            GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_FREEZE1);
            gameObject.tag = "Freeze";
        }
        /*else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = 9;
            if (GreenEnemyAnimator.GetInteger("GreenEnemyCurrentState") == STATE_FREEZE1)
                GreenEnemyAnimator.SetInteger("GreenEnemyCurrentState", STATE_IDLE);
        }*/

    }

    public IEnumerator Attack1()
    {
      //  if (transform.localScale.x > 0)
        {
            GameObject newObject =Instantiate(bulletFire, shootPointHorizontal.position, Quaternion.identity);
            if (transform.localScale.x < 0)
                newObject.transform.localScale = new Vector2(-1, newObject.transform.localScale.y);
        }

        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator Attack2()
    {
        //  if (transform.localScale.x > 0)
        {
            GameObject newObject = Instantiate(bulletFire, shootPointVertical.position, Quaternion.identity);
            // if (transform.localScale.x < 0)
            newObject.transform.Rotate(0, 0, -90);
        }

        yield return new WaitForSeconds(.5f);
    }

}
