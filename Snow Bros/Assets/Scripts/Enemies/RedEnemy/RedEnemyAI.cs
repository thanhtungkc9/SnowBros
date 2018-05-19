using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyAI : MonoBehaviour {

    private Enemy enemy;
    private Animator anim;
    private Transform playerTranform;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tuong" && !GetComponent<EnemyFreeze>().isFreeze)
        {
            if (enemy.flyAI || !enemy.sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                ChangeDirection();//khi enemy dung cao hon player se tim noi de nhay xuong neu dung phai tuong se quay dau
        }
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
        }
    }

    void Live()
    {
        if (!GetComponent<EnemyFreeze>().isFreeze)
        {
            enemy.timeAI += Time.deltaTime;
            if (enemy.ground)
            {
                enemy.Move();
                if (enemy.timeAI > .5f)
                {
                    anim.SetBool("Roll", false);
                }

                if (enemy.timeAI > 3f)
                {
                    enemy.sizeY = transform.position.y - playerTranform.position.y;
                    if (enemy.sizeY < -1f && enemy.sizeJump.GetComponent<SizeJumpEnemy>().sizeJump)
                    {
                        enemy.timeAI = 0;
                        enemy.Jump();
                    }
                    else if (enemy.sizeY > -1f && enemy.sizeY < 1f)
                    {
                        enemy.timeAI = 0;
                        anim.SetBool("Roll", true);
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
