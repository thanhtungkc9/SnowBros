using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public float jumpForce = 700f;
    public float moveXVelocity = 100f;
    private bool grounded;
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
    public int CURRENTSTATE = 0;
    private Rigidbody2D playerBody;
    private Animator playerAnimator;
    
    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      switch (CURRENTSTATE)
        {
            case STATE_IDLE:
                Idle();
                break;
            case STATE_WALK:
            case STATE_PUSH:
                Walk_Push();
                break;
            case STATE_THROW:
                playerAnimator.SetInteger("CurrentState", STATE_IDLE);
                CURRENTSTATE = STATE_IDLE;
                break;
            case STATE_RUNTHROW:
                playerAnimator.SetInteger("CurrentState", STATE_IDLE);
                CURRENTSTATE = STATE_IDLE;
                break;
        }
	}

    void Keyboard()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
        }
        else if (h<0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }
        else
        {
            playerAnimator.SetBool("Walk", false);
        }
        if (Input.GetKey(KeyCode.J))
        {
            if (playerAnimator.GetBool("Jump")|| !playerAnimator.GetBool("Walk"))
            playerAnimator.SetBool("Throw", true);
            else if(playerAnimator.GetBool("Walk"))
                playerAnimator.SetBool("RunThrow", true);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {

            playerAnimator.SetBool("RunThrow", false);
            playerAnimator.SetBool("Throw", false);
        }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        
        if (target.gameObject.tag=="SnowBall"&&CURRENTSTATE!=STATE_PUSH&&CURRENTSTATE!=STATE_IDLE)
        {
            Debug.Log("Collision with Snowball");
            playerAnimator.SetInteger("CurrentState", STATE_PUSH);
            CURRENTSTATE = STATE_PUSH;
        }
    }
    void OnCollisionExit2D(Collision2D target)
    {

       
    }
    void Idle()
    {
        playerBody.velocity = new Vector2(0, 0);
        float h = Input.GetAxisRaw("Horizontal");
            if (h != 0&& CURRENTSTATE != STATE_WALK)
            {
                playerAnimator.SetInteger("CurrentState", 1);
                CURRENTSTATE = STATE_WALK;
            }

        
        if (Input.GetKey(KeyCode.J))
        {
            
            playerAnimator.SetInteger("CurrentState", STATE_THROW);
            CURRENTSTATE = STATE_THROW;
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {

            playerAnimator.SetInteger("CurrentState", STATE_JUMP);
            CURRENTSTATE = STATE_JUMP;
        }
   
    }
    void Walk_Push()
    {

        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
            playerBody.velocity=new Vector2(moveXVelocity * Time.deltaTime, 0);
        }
        else if (h<0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
            playerBody.velocity = new Vector2(-moveXVelocity * Time.deltaTime, 0);
        }
        if (h==0)
        {
            playerAnimator.SetInteger("CurrentState", 0);
            CURRENTSTATE = STATE_IDLE;
        }
        if (Input.GetKey(KeyCode.J))
        {

            playerAnimator.SetInteger("CurrentState", STATE_RUNTHROW);
            CURRENTSTATE = STATE_RUNTHROW;
        }
    }


}
