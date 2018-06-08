using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenVerticalEnemyAI : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    public int STATE_ATTACK2 = 12;
   

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.2f&&player.transform.position.y<transform.position.y)
            GetComponent<Animator>().SetInteger("GreenEnemyCurrentState", 12);
    }



}
