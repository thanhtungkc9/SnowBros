using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreeze : MonoBehaviour {

    [SerializeField]
    private PhysicsMaterial2D material;
    public bool isFreeze;

    private Rigidbody2D myBody;
    private Animator anim;

    private float timeFreeze = 0;
    private int lvFreeze = 0, damage = 0;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (isFreeze)
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

    void Damage(int dmg)
    {
        timeFreeze = 0;
        damage = dmg;
        if (lvFreeze <= 100)
        {
            lvFreeze += damage;
        }
        AnimationFreeze(lvFreeze);
    }
}
