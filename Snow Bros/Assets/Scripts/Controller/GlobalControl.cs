using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    //Player Information
    public static int DeathCount = 0;
    public static int Score = 0;
    public static float maxVelocity = 3.0f;     //4.5f with Item
    public static float jumpForce = 1700.0f;
    public static float moveForce = 150.0f;

    //PlayerBullet
    public static float mass = 0.3f;        //0.2 with Item
    public static float timeExist = 0.4f;   // 0.6 with Item
    public static int damage = 10;          //15 with Item
    public static string spriteName = "SmallBullet"; // BigBullet with Item

    //Scene Information
    public static Scene[] allScene = SceneManager.GetAllScenes();
    public static int CurrentScene = 0;



    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Player_SpeedItem()
    {
        maxVelocity = 4.5f;
    }
    public void Player_PowerItem()
    {
        damage = 15;
        spriteName = "BigBullet";
    }
    public void Player_RangeItem()
    {
        mass = 0.2f;
        timeExist = 0.6f;
    }
    public void Player_ResetItem()
    {
        moveForce = 150.0f;
        mass = 0.3f;
        timeExist = 0.4f;
        damage = 10;
        spriteName = "SmallBullet";
    }
    public void Score_Add(int bonus)
    {
        Score += bonus;
    }

}
