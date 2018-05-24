using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss1 : MonoBehaviour {
    private Animator boss1Animator;
    public Rigidbody2D boss1Body;

    public bool grounded = false;

    public int STATE_IDLE = 0;
    public int STATE_ATTACK = 1;
    public int STATE_JUMP = 2;
    public int STATE_DIE = 3;
    public int LAST_ACTION = 1;

    public float jumpForce=2100.0f;

    [SerializeField]
    public GameObject enemyBoss1;
    // Use this for initialization
    void Start () {

    }
    void Awake()
    {
        boss1Animator = GetComponent<Animator>();
        boss1Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D target)
    {
     
        if ((target.gameObject.tag == "Ground"|| target.gameObject.tag == "Wall") && boss1Body.velocity.y < 0.1f)
        {
            grounded = true;
            boss1Animator.SetInteger("Boss1CurrentState", STATE_IDLE);
        }
    }
    void OnCollisionExit2D(Collision2D target)
    {


        if ((target.gameObject.tag == "Ground"))
        {
            grounded = false;
        }
    }
}
