using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; private set; }
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public static void BackToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
