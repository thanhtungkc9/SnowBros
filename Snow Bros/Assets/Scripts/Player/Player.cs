using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveForce = 150f;
    public float jumpForce = 1400f;
    public float maxVelocity = 3f;
    public float pushForce = 1f;
    public float damage = 10f;
    public Transform shootPoint;

    private bool grounded, pushed, bullet = false, keyupJ = true, keyupK = true;
    private Rigidbody2D myBody;
    private Animator anim;
    [SerializeField]
    private GameObject bullet1;
    [SerializeField]
    private GameObject bullet2;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        shootPoint = transform.Find("ShootPoint");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        PlayerKeyBoard();
	}

    void PlayerKeyBoard()
    {
        float forceX = 0f;
        float forceY = 0f;
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
        {
            if (myBody.velocity.x < maxVelocity)
            {
                if (grounded)
                {
                    forceX = moveForce;
                    if (pushed)
                    {
                        anim.SetBool("Push", true);
                    }
                    else
                    {
                        anim.SetBool("Walk", true);
                    }
                }
                else
                {
                    forceX = moveForce * 0.5f;
                }
            }
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
        }
        else if (h < 0)
        {
            if(myBody.velocity.x > -maxVelocity)
            {
                if (grounded)
                {
                    forceX = -moveForce;
                    if (pushed)
                    {
                        anim.SetBool("Push", true);
                    }
                    else
                    {
                        anim.SetBool("Walk", true);
                    }
                }
                else
                {
                    forceX = -moveForce * 0.5f;
                }
            }
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }
        else if (h == 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Push", false);
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }

        if (Input.GetKey(KeyCode.K) && !anim.GetBool("Throw"))
        {
            if (grounded && keyupK && !anim.GetBool("Jump"))
            {
                grounded = false;
                keyupK = false;
                forceY = jumpForce;
                anim.SetBool("Walk", false);
                anim.SetBool("Push", false);
                anim.SetBool("Jump", true);
            }
        }
        else if (Input.GetKey(KeyCode.J))
        {
            if (!anim.GetBool("Fly") && !anim.GetBool("Push") && !anim.GetBool("Jump") && keyupJ)
            {
                keyupJ = false;
                anim.SetBool("Throw", true);
                StartCoroutine(Attack());
            }
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            keyupK = true;
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            keyupJ = true;
            anim.SetBool("Throw", false);
        }

        myBody.AddForce(new Vector2(forceX, forceY));
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Freeze")
        {
            grounded = true;
            anim.SetBool("Fly", false);
            //xử lý việc khi nhảy xuyên qua ground trong trạng thái đang rơi mới set jump = false
            if (myBody.velocity.y <= 0)
            {
                anim.SetBool("Jump", false);
                if(collision.gameObject.tag == "Freeze")
                {
                    pushed = false;
                }
            }
        }
        if (collision.gameObject.tag == "Freeze4")
        {
            if(transform.localScale.x == 1f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(pushForce * 500f, 0), transform.position);
            }else if (transform.localScale.x == -1f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(-pushForce * 500f, 0), transform.position);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Freeze")
        {
            pushed = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(pushForce, gameObject.GetComponent<Rigidbody2D>().velocity.y));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //xử lý animation fly
        if (collision.gameObject.tag == "Ground" && myBody.velocity.y < -3f)
        {
            if (!anim.GetBool("Jump"))
            {
                anim.SetBool("Fly", true);
            }
        }
        if (collision.gameObject.tag == "Freeze")
        {
            pushed = false;
            anim.SetBool("Push", false);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
