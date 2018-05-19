using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour {

    private Enemy enemy;
    private CircleCollider2D cirCollider;
    private Rigidbody2D myBody;
    private Animator anim;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cirCollider = GetComponent<CircleCollider2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6.66f, 8.9f), transform.position.y);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !enemy.isLive)
        {
            anim.SetInteger("Die", 2);
            Destroy(gameObject, .5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Die" && !enemy.isLive)
        {
            cirCollider.isTrigger = false;
            myBody.freezeRotation = true;
        }
    }

    public void Die()
    {
        enemy.isLive = false;
        cirCollider.isTrigger = true;
        myBody.freezeRotation = false;
        myBody.gravityScale = 5;
        myBody.AddForce(new Vector2(Random.Range(-500f, 500f), 700f));
        anim.SetBool("Walk", true);
        anim.SetInteger("Die", 1);
    }
}
