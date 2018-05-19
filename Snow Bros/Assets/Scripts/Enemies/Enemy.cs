using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform startPos, endPos;
    public GameObject sizeJump;
    public float speed = 7f;
    public float forceY = 2000f;

    public float timeAI = 0;
    public float sizeY;
    public bool collision;
    public bool ground = true;
    public bool flyAI = false; //xử lý việc AI của enemy khi rơi
    public bool isLive = true;

    private Rigidbody2D myBody;
    private Animator anim;
    private CircleCollider2D cirCollider;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cirCollider = GetComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && isLive)
        {
            ground = true;
            anim.SetBool("Fly", false);
            if (myBody.velocity.y <= 0)
            {
                anim.SetBool("Jump", false);
            }
        }

        if (gameObject.tag != "Freeze4")
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Freeze")
            {
                //2 enemy di cùng chiều nhau thì 1 trong 2 con sẽ đổi chiều
                if (transform.localScale.x == collision.transform.localScale.x)
                {
                    if (transform.localScale.x == 1f)
                    {
                        Vector2 temp = transform.localScale;
                        temp.x = -1f;
                        transform.localScale = temp;
                    }
                    else if (collision.transform.localScale.x == 1f)
                    {
                        Vector2 temp = collision.transform.localScale;
                        temp.x = -1f;
                        collision.transform.localScale = temp;
                    }
                }
                //2 enemy đi ngược chiều nhau thì sẽ lướt qua nhau
                else
                {
                    myBody.gravityScale = 0;
                    cirCollider.isTrigger = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!anim.GetBool("Jump") && myBody.velocity.y < -3f)
            {
                ground = false;
                anim.SetBool("Fly", true);
                if (flyAI)
                    timeAI = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Freeze")
        {
            myBody.gravityScale = 20;
            cirCollider.isTrigger = false;
        }
    }

    public void Move()
    {
        if (!anim.GetBool("Jump") && !anim.GetBool("Jump"))
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
            myBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        }
    }

    public void Jump()
    {
        ground = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", true);
        myBody.velocity = new Vector2(0, myBody.velocity.y);
        myBody.AddForce(new Vector2(0, forceY));
    }
}
