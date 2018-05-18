using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeJumpEnemy : MonoBehaviour {

    public bool sizeJump;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            sizeJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            sizeJump = false;
        }
    }
}
