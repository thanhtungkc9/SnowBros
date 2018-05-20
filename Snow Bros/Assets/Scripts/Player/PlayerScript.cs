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
    public int CURRENTSTATE = 0;

    private Rigidbody2D playerBody;
    private Animator playerAnimator;
    public Transform shootPoint;
    [SerializeField]
    private GameObject bullet1;
    [SerializeField]
    private GameObject bullet2;

    public float jumpForce = 700f;
    public float moveXVelocity = 100f;
    public float moveForce = 150f;
    public float maxVelocity = 8f;
    private bool grounded, pushed, bullet = false, keyupJ = true, keyupK = true;
    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start() {
        shootPoint = transform.Find("ShootPoint");
    }

    // Update is called once per frame
    void Update() {
        Keyboard_Move();
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
            case STATE_RUNTHROW:
                Throw_RunThrow();
                break;
            case STATE_JUMP:
                Jump();
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
        else if (h < 0)
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
            if (playerAnimator.GetBool("Jump") || !playerAnimator.GetBool("Walk"))
                playerAnimator.SetBool("Throw", true);
            else if (playerAnimator.GetBool("Walk"))
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

        if (target.gameObject.tag == "SnowBall" && CURRENTSTATE != STATE_PUSH && CURRENTSTATE != STATE_IDLE)
        {
            Debug.Log("Collision with Snowball");
            playerAnimator.SetInteger("CurrentState", STATE_PUSH);
            CURRENTSTATE = STATE_PUSH;
        }
        if (target.gameObject.tag == "Ground" || target.gameObject.tag == "Freeze")
        {
            grounded = true;
            float h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                playerAnimator.SetInteger("CurrentState", STATE_WALK);
                CURRENTSTATE = STATE_WALK;
            }
            else
            {
                playerAnimator.SetInteger("CurrentState", STATE_IDLE);
                CURRENTSTATE = STATE_IDLE;
            }
        }
    }
    void OnCollisionExit2D(Collision2D target)
    {
        //xử lý animation fly
        if (target.gameObject.tag == "Ground" && playerBody.velocity.y < -3f)
        {
            grounded = false;
        }

    }
    void Idle()
    {
        playerBody.velocity = new Vector2(0, playerBody.velocity.y);
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0 && CURRENTSTATE != STATE_WALK)
        {
            playerAnimator.SetInteger("CurrentState", 1);
            CURRENTSTATE = STATE_WALK;
        }


        if (Input.GetKey(KeyCode.J))
        {

            {
                playerAnimator.SetInteger("CurrentState", STATE_THROW);
                CURRENTSTATE = STATE_THROW;
            }

        }
        else if (Input.GetKey(KeyCode.K) )
        {
            if (grounded && keyupK && playerAnimator.GetInteger("CurrentState")!=STATE_JUMP)
            {
                grounded = false;
                keyupK = false;
                playerAnimator.SetInteger("CurrentState", STATE_JUMP);
                CURRENTSTATE = STATE_JUMP;
            }
        }

    }

    void Keyboard_Move()
    {
        if (CURRENTSTATE == STATE_DIE || CURRENTSTATE == STATE_RESPAWN || CURRENTSTATE == STATE_FLY)
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
        }
        else if (h == 0)
        {
            forceX = 0;
        }
        playerBody.AddForce(new Vector2(forceX, forceY));

    }

    void Jump()
    {
        if (keyupK == false)
        {
            playerBody.AddForce(new Vector2 (0, jumpForce));
            keyupK = true;

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

        }
        else if (h < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }
        else if (h == 0)
            {
                playerAnimator.SetInteger("CurrentState", 0);
                CURRENTSTATE = STATE_IDLE;
            }
        if (Input.GetKey(KeyCode.J))
        {

            playerAnimator.SetInteger("CurrentState", STATE_RUNTHROW);
            CURRENTSTATE = STATE_RUNTHROW;
        }
        else if (Input.GetKey(KeyCode.K))
        {

            if (grounded && keyupK && playerAnimator.GetInteger("CurrentState") != STATE_JUMP)
            {
                grounded = false;
                keyupK = false;
                playerAnimator.SetInteger("CurrentState", STATE_JUMP);
                CURRENTSTATE = STATE_JUMP;
            }
        }
    }

    void Throw_RunThrow()
    {
        if (keyupJ)
        {
            keyupJ = false;
            playerAnimator.SetBool("Throw", true);
            StartCoroutine(Attack());
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            keyupJ = true;
            playerAnimator.SetInteger("CurrentState", 0);
            CURRENTSTATE = STATE_IDLE;
        }
    }
    IEnumerator Attack()
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
        yield return new WaitForSeconds(.3f);
    }

}
