using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {


    public const int STATE_IDLE = 0;
    public const int STATE_WALK = 1;
    public const int STATE_JUMP = 2;
    public const int STATE_THROW = 3;
    public const int STATE_PUSH = 4;
    public const int STATE_KICK = 5;
    public const int STATE_RUNPUSH = 6;
    public const int STATE_RUN = 7;
    public const int STATE_FLY = 8;
    public const int STATE_DIE = 9;
    public const int STATE_RESPAWN = 10;
    public const int STATE_RUNTHROW = 11;

    public Rigidbody2D playerBody;
    private Animator playerAnimator;
    public Transform shootPoint;
    [SerializeField]
    private GameObject bullet1;
    [SerializeField]
    private GameObject bullet2;

    public float jumpForce = 1700f;
    public float moveForce = 150f;
    public float maxVelocity = 3f;
    public bool grounded = false, bullet = false;
    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start() {
    }
    // Update is called once per frame
    void FixedUpdate() {
        Keyboard_Move();   
       
    }

    void OnCollisionEnter2D(Collision2D target)
    {

        if (target.gameObject.tag == "Freeze4"  && playerBody.velocity.x != 0)
        {
            Debug.Log("Collision SnowBall");
          //  if (playerAnimator.GetInteger("CurrentState") != STATE_PUSH)
           //     playerAnimator.SetInteger("CurrentState", STATE_PUSH);
          //  else
            {
                Debug.Log("Add force");
                target.collider.SendMessageUpwards("PlayerKicked", transform.localScale.x);
                //if (transform.localScale.x == 1f)
                //{
                //    target.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(1000.0f, 0), Vector2.zero);
                //}
                //else if (transform.localScale.x == -1f)
                //{
                //    target.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(-1000.0f, 0), Vector2.zero);
                //}
            }
        }
      //  Debug.Log(playerBody.velocity.y);
        if ((target.gameObject.tag == "Ground" || target.gameObject.tag == "Freeze4") && playerBody.velocity.y < 0.1f)
        {
            grounded = true;
            float h = Input.GetAxisRaw("Horizontal");
             if (h != 0)
            {
                playerAnimator.SetInteger("CurrentState", STATE_WALK);
            }
            else 
            {

                playerAnimator.SetInteger("CurrentState", STATE_IDLE);

            }
        }
        if (target.gameObject.tag=="Enemy")
        {
            gameObject.layer = 14;
            playerAnimator.SetInteger("CurrentState", STATE_DIE);
        }
    }

    void OnCollisionExit2D(Collision2D target)
    {
        if ((target.gameObject.tag=="Ground" || target.gameObject.tag == "Freeze4") && playerBody.velocity.y < -0.2f)
        {
            grounded = false;
        }
             if (target.gameObject.tag == "Freeze4" && playerAnimator.GetInteger("CurrentState") == STATE_PUSH)
        {
            playerAnimator.SetInteger("CurrentState", STATE_WALK);
        }
    }
    void OnCollisionStay2D(Collision2D target)
    {
    
        if (target.gameObject.tag == "Freeze4" &&playerAnimator.GetInteger("CurrentState")==STATE_WALK)
        {
            playerAnimator.SetInteger("CurrentState", STATE_PUSH);
        }
    }

    void Keyboard_Move()
    {
        int currentState = playerAnimator.GetInteger("CurrentState");
        if (currentState == STATE_DIE || currentState == STATE_RESPAWN || currentState == STATE_FLY)
            return;
        float h = Input.GetAxisRaw("Horizontal");
        float forceX = 0f;
        float forceY = 0f;

        if (h > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
            if (playerBody.velocity.x < maxVelocity)
            {
                if (grounded)
                {
                    forceX = moveForce;
                }
                else
                {
                    forceX = moveForce * 0.5f;
                }
            }
            playerBody.AddForce(new Vector2(forceX, forceY));
        }
        else if (h < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
            if (playerBody.velocity.x > -maxVelocity)
            {
                if (grounded)
                {
                    forceX = -moveForce;
                }
                else
                {
                    forceX = -moveForce * 0.5f;
                }
            }
            playerBody.AddForce(new Vector2(forceX, forceY));
        }
        else if (h == 0)
        {
            playerBody.velocity=new Vector2(0,playerBody.velocity.y);
        }
       

    }

 
    public IEnumerator Attack()
    {
        if (bullet)
        {
            bullet = !bullet;
            Instantiate(bullet1, shootPoint.position, Quaternion.identity);
        }
        else
        {
            bullet = !bullet;
            Instantiate(bullet2, shootPoint.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(.1f);
    }

}
