using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; private set; }
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        GlobalControl.isPaused = false;
        Time.timeScale = 1;
    }
    public static void LoadScene(int index)
    {
        GlobalControl.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }
    public static void BackToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
        GlobalControl.isPaused = false;
        Time.timeScale = 1;
    }
}
