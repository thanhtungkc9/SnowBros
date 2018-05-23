using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemyAI : MonoBehaviour {

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletPos;
    public bool attack = false; //kiểm tra xem enemy có đang tấn công hay không

    private Enemy enemy;
    private Animator anim;
    private Transform playerTranform;

    private bool bulleting = false; //xử lý việc chỉ cho enemy bắn 1 viên đạn 1 lần

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        playerTranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isLive)
        {
            Live();
        }
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && !GetComponent<EnemyFreeze>().isFreeze)
        {
            if (enemy.flyAI || !enemy.sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                ChangeDirection();//khi enemy dung cao hon player se tim noi de nhay xuong neu dung phai tuong se quay dau
        }
    }

    void DelayAttack()
    {
        attack = false;
    }

    void ChangeDirection()
    {
        enemy.collision = Physics2D.Linecast(enemy.startPos.position, enemy.endPos.position, 1 << LayerMask.NameToLayer("Ground"));
        if (!enemy.collision)
        {
            Vector3 temp = transform.localScale;
            if (temp.x == 1f)
            {
                temp.x = -1f;
            }
            else
            {
                temp.x = 1f;
            }
            transform.localScale = temp;
            attack = true;
            bulleting = false;
            enemy.timeAI = 0;
            StartCoroutine(Attack());
            Invoke("DelayAttack", 2f);
        }
    }

    void Live()
    {
        if (!GetComponent<EnemyFreeze>().isFreeze)
        {
            enemy.timeAI += Time.deltaTime;
            if (enemy.ground && !attack)
            {
                enemy.Move();
                if (enemy.timeAI > 7f)
                {
                    enemy.sizeY = transform.position.y - playerTranform.position.y;
                    if (enemy.sizeY < -1f && enemy.sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                    {
                        enemy.timeAI = 0;
                        enemy.Jump();
                    }
                    else if (enemy.sizeY > -1f && enemy.sizeY < 1f)
                    {
                        attack = true;
                        enemy.timeAI = 0;
                        bulleting = false;
                        StartCoroutine(Attack());
                        Invoke("DelayAttack", 2f);
                    }
                    else if (enemy.sizeY > 1f)
                    {
                        enemy.flyAI = true;
                    }
                }
                else if (!enemy.flyAI)
                {
                    ChangeDirection();
                }
            }
        }
    }
}
