using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private Transform startPos, endPos;
    private bool collision, ground = true, isFreeze = false;
    private float timeDirection = 0, timeJump = 0, timeFreeze = 0;
    private int lvFreeze = 0, damage = 0;

    public float speed = 7f;
    public float forceY = 2000f;

    private Rigidbody2D myBody;
    private Animator anim;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isFreeze)
        {
            timeDirection += Time.deltaTime;
            timeJump += Time.deltaTime;
            if (ground)
            {
                Move();
                if (timeJump > 7f)
                {
                    Jump();
                    timeJump = 0;
                }
                else if (timeDirection > .1f && timeDirection < 5f)
                {
                    ChangeDirection();
                }
            }
        }
        else
        {
            timeFreeze += Time.deltaTime;
            if(timeFreeze > 2f)
            {
                if(lvFreeze > 0)
                {
                    timeFreeze = 1f;
                    lvFreeze -= damage * 3;
                    AnimationFreeze(lvFreeze);
                }
                else
                {
                    isFreeze = false;
                    anim.SetBool("Freeze1", false);
                    gameObject.tag = "Enemy";
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (myBody.velocity.y <= 0)
            {
                ground = true;
                timeDirection = 0;
                anim.SetBool("Jump", false);
                anim.SetBool("Fly", false);
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
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }

    void Move()
    {
        if (!anim.GetBool("Jump") && !anim.GetBool("Jump"))
        {
            myBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
            anim.SetBool("Walk", true);
        }
    }

    void Jump()
    {
        ground = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", true);
        myBody.AddForce(new Vector2(0, forceY));
    }

    void ChangeDirection()
    {
        collision = Physics2D.Linecast(startPos.position, endPos.position, 1 << LayerMask.NameToLayer("Ground"));
        if (!collision)
        {
            Vector3 temp = transform.localScale;
            if(temp.x == 1f)
            {
                temp.x = -1f;
            }
            else
            {
                temp.x = 1f;
            }
            transform.localScale = temp;
        }
    }

    void Damage(int dmg)
    {
        timeFreeze = 0;
        damage = dmg;
        if(lvFreeze <= 100)
        {
            lvFreeze += damage;
        }
        AnimationFreeze(lvFreeze);
    }

    void AnimationFreeze(int lvfreeze)
    {
        if (ground)
        {
            isFreeze = true;
            if (lvfreeze >= 10 && lvfreeze <= 30)
            {
                anim.SetBool("Freeze2", false);
                anim.SetBool("Freeze1", true);
            }
            else if (lvfreeze >= 40 && lvfreeze <= 60)
            {
                anim.SetBool("Freeze3", false);
                anim.SetBool("Freeze2", true);
            }
            else if (lvfreeze >= 70 && lvfreeze <= 90)
            {
                anim.SetBool("Freeze4", false);
                anim.SetBool("Freeze3", true);
            }
            else if (lvfreeze == 100)
            {
                anim.SetBool("Freeze4", true);
                gameObject.tag = "Freeze";
            }
        }
    }
}
