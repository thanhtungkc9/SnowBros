using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler {

    private PlayerScript player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
      
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (gameObject.name == "LeftButton")
        {
            player.isMoveLeft=true;
            player.isMoveRight = false;
            Debug.Log("Left Clicked");
        }
        else if (gameObject.name == "RightButton")
        {
            player.isMoveLeft = false;
            player.isMoveRight = true;
        }
        else if (gameObject.name == "ShootButton")
        {
            player.isShoot = true;

        }
        else if (gameObject.name == "JumpButton")
        {
            player.isJump = true;

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.name == "LeftButton")
        {
            player.isMoveLeft = false;
           
        }
        else if (gameObject.name == "RightButton")
        {
            player.isMoveRight = false;
        }
        else if (gameObject.name == "ShootButton")
        {
            player.isShoot = false;

        }
        else if (gameObject.name == "JumpButton")
        {
            player.isJump = false;

        }
    }
}
