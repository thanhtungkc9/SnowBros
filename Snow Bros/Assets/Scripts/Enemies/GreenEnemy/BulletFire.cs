using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour {

    public float speed = 8f;

    private bool direction;
    private GameObject enemy;

    private void Awake()
    {
        enemy = GameObject.Find("GreenEnemy");
    }

    // Use this for initialization
    void Start () {
		if(transform.localScale.x == 1f)
        {
            direction = true;
        }
        else if (transform.localScale.x == -1f)
        {
            direction = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(FLy());
        Destroy(gameObject, .8f);
    }

    IEnumerator FLy()
    {
        if (direction)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
            Vector2 newPos1 = transform.position;
            newPos1 = new Vector2(newPos1.x + speed * Time.deltaTime, newPos1.y);
            transform.position = newPos1;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
            Vector2 newPos2 = transform.position;
            newPos2 = new Vector2(newPos2.x - speed * Time.deltaTime, newPos2.y);
            transform.position = newPos2;
        }
        yield return new WaitForSeconds(.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tuong" || collision.gameObject.tag == "Player")
        {
            Debug.Log("BulletFire collider");
            Destroy(gameObject);
        }
    }
}
