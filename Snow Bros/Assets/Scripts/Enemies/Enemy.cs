using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private Transform startPos, endPos, bulletPos;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject sizeJump;
    [SerializeField]
    private PhysicsMaterial2D material;
    private bool collision, ground = true, isFreeze = false, attack = false;
    private bool flyAI = false; //xử lý việc AI của enemy khi rơi
    private bool bulleting = false; //xử lý việc chỉ cho enemy bắn 1 viên đạn 1 lần
    private float timeAI = 0, timeFreeze = 0, timeAttack = 0;
    private int lvFreeze = 0, damage = 0;
    private float timeRdAI = 3f;

    public float speed = 7f;
    public float forceY = 2000f;
    public bool isLive = true;

    private Rigidbody2D myBody;
    private Animator anim;
    private CircleCollider2D cirCollider;
    private Transform playerTranform;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cirCollider = GetComponent<CircleCollider2D>();
        playerTranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isLive)
        {
            Live();
        }
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
        else if(collision.gameObject.tag == "Ground" && !isLive)
        {
            anim.SetBool("Die2", true);
            Destroy(gameObject, .5f);
        }
        if (collision.gameObject.tag == "Tuong")
        {
            if(flyAI || !sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                ChangeDirection();//khi enemy dung cao hon player se tim noi de nhay xuong neu dung phai tuong se quay dau
        }
        if (collision.gameObject.tag == "Enemy")
        {
            myBody.gravityScale = 0;
            cirCollider.isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!anim.GetBool("Jump") && myBody.velocity.y < -3f)
            {
                ground = false;
                if (flyAI)
                {
                    timeAI = 0;
                }
                anim.SetBool("Fly", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Die" && !isLive)
        {
            cirCollider.isTrigger = false;
            myBody.freezeRotation = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            myBody.gravityScale = 20;
            cirCollider.isTrigger = false;
        }
    }

    void Move()
    {
        if (!anim.GetBool("Jump") && !anim.GetBool("Jump"))
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
            myBody.velocity = new Vector2(transform.localScale.x, 0) * speed;
        }
    }

    void Jump()
    {
        ground = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", true);
        myBody.velocity = new Vector2(0, myBody.velocity.y);
        myBody.AddForce(new Vector2(0, forceY));
    }

    IEnumerator Attack()
    {
        anim.SetBool("Walk", false);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Attack", true);
        if (!bulleting)
        {
            bulleting = true;
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(.1f);
        timeAttack = 0;
        attack = false;
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
            gameObject.tag = "Freeze";
            myBody.sharedMaterial = null;
            myBody.freezeRotation = true;
            transform.localRotation = Quaternion.identity;
        }
        else if (lvfreeze == 100)
        {
            anim.SetBool("Freeze4", true);
            gameObject.tag = "Freeze4";
            myBody.sharedMaterial = material;
            myBody.freezeRotation = false;
        }
    }

    void Live()
    {
        if (!isFreeze)
        {
            timeAI += Time.deltaTime;
            if (bullet != null && bulletPos != null)
            {
                timeAttack += Time.deltaTime;
            }
            if (ground)
            {
                if (!attack)
                {
                    Move();
                    if (timeAI > .5f)
                    {
                        anim.SetBool("Roll", false);
                    }
                    if (timeAI > timeRdAI)
                    {
                        float sizeY = transform.position.y - playerTranform.position.y;
                        if (sizeY < -1f && sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                        {
                            timeAI = 0;
                            Jump();
                        }
                        else if (sizeY > -1f && sizeY < 1f)
                        {
                            timeAI = 0;
                            anim.SetBool("Roll", true);
                        }
                        else if (sizeY > 1f)
                        {
                            flyAI = true;
                        }
                    }
                    else if(!flyAI)
                    {
                        ChangeDirection();
                    }

                    if (timeAttack > Random.Range(10f, 15f))
                    {
                        attack = true;
                        bulleting = false;
                    }
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
        }
        else
        {
            timeFreeze += Time.deltaTime;
            if (timeFreeze > 5f)
            {
                if (lvFreeze > 0)
                {
                    timeFreeze = 0f;
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

    public void Die()
    {
        isLive = false;
        cirCollider.isTrigger = true;
        myBody.freezeRotation = false;
        myBody.gravityScale = 10;
        myBody.AddForce(new Vector2(Random.Range(-500, 500f), forceY));
        anim.SetBool("Die1", true);
    }
}
