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

    public float jumpForce = 1550f;
    public float moveXVelocity = 100f;
    public float moveForce = 150f;
    public float maxVelocity = 3f;
    public bool grounded, bullet = false, keyupJ = true, keyupK = true;
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

        if (target.gameObject.tag == "Freeze4" && playerAnimator.GetInteger("CurrentState") != STATE_PUSH && playerBody.velocity.x!=0)
        {
            playerAnimator.SetInteger("CurrentState", STATE_PUSH);
        }
        if ((target.gameObject.tag == "Ground" || target.gameObject.tag == "Freeze4"))
        {
            grounded = true;

            Debug.Log("Collision with ground");
            float h = Input.GetAxisRaw("Horizontal");
             if (h != 0)
            {
                playerAnimator.SetInteger("CurrentState", STATE_WALK);
            }
            else if (h == 0)
            {

                playerAnimator.SetInteger("CurrentState", STATE_IDLE);

            }
        }
    }

    void OnCollisionExit2D(Collision2D target)
    {
        //
        if (target.gameObject.tag == "Ground" && playerBody.velocity.y < -3f)
        {
            grounded = false;
        }
        else
             if (target.gameObject.tag == "Freeze4" && playerAnimator.GetInteger("CurrentState") == STATE_PUSH)
        {
            playerAnimator.SetInteger("CurrentState", STATE_WALK);
        }
    }
    void OnCollisionStay2D(Collision2D target)
    {
        //
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
