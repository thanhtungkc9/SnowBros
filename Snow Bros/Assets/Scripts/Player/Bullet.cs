using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 8f;
    public int dmg = 10;
    public float forceX=120.0f;
    public float forceY=15.0f;
    public float timeExist = 0.4f;
    public Sprite bigBullet;

    private bool direction;
    private GameObject player;
    private Rigidbody2D bullet;

    
    private float timeFly = 0;
    private void Awake()
    {
        player = GameObject.Find("Player");
        bullet = GetComponent<Rigidbody2D>();
        Bullet_LoadData();
    }

    // Use this for initialization
    void Start () {

        if (player.transform.localScale.x == 1f)
        {
            direction = true;
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
            bullet.AddForce(new Vector2(forceX, forceY));
        }
        else if (player.transform.localScale.x == -1f)
        {
            direction = false;
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
            ;
            bullet.AddForce(new Vector2(-forceX, forceY));
          //  bullet.velocity = new Vector2(-1f, 0f);

        }
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Fly());
        timeFly += Time.deltaTime;
        if (timeFly >= timeExist)
        {
            bullet.mass = 5;
            bullet.velocity = new Vector2(0, 0);
            bullet.gravityScale = 20;
        }
    }

    IEnumerator Fly()
    {
        if (direction)
        {

            
        }
        else
        {

            
        }
        yield return new WaitForSeconds(.2f);
        /*
        if (direction)
        {
           
            Vector2 newPos1 = transform.position;
            newPos1 = new Vector2(newPos1.x + speed * Time.deltaTime, newPos1.y);
            transform.position = newPos1;
        }
        else
        {
           
            Vector2 newPos2 = transform.position;
            newPos2 = new Vector2(newPos2.x - speed * Time.deltaTime, newPos2.y);
            transform.position = newPos2;
        }
        yield return new WaitForSeconds(.2f);
       // Vector2 newPos = transform.position;
      //  newPos = new Vector2(newPos.x, newPos.y - speed * Time.deltaTime);
       // transform.position = newPos;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Item" || collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
            
        if(collision.gameObject.tag == "Enemy" 
            || collision.gameObject.tag == "Freeze"
            || collision.gameObject.tag == "Boss"
            || collision.gameObject.tag == "Freeze4")
        {

            if (collision.gameObject.layer != 14)
            {
                collision.SendMessageUpwards("Damage", dmg);
                Destroy(gameObject);
            }
        }
    }

    private void Bullet_LoadData()
    {
        dmg = GlobalControl.damage;
        timeExist = GlobalControl.timeExist;
        GetComponent<Rigidbody2D>().mass = GlobalControl.mass;
        if (GlobalControl.spriteName=="BigBullet") GetComponent<SpriteRenderer>().sprite=bigBullet;
    }
}
