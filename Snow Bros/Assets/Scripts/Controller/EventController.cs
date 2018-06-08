using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EventController : MonoBehaviour {

    [SerializeField]
    private Image menu;

    private void Start()
    {

    }
    public void OnPlayButtonClick()
    {
        SceneController.LoadScene("SelectStage");
    }
    public void OnScoresButtonClick()
    {
        
    }
    public void OnSettingsButtonClick()
    {
        
    }

    public void OnStage1ButtonClick()
    {
        SceneController.LoadScene("Stage1-1");
    }

    public void BackToMenuScene()
    {
        SceneController.BackToMenuScene();
    }
    public void ReloadScene()
    {
        SceneController.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowMenu()
    {
        menu.gameObject.active = true;
        Time.timeScale = 0;
        GlobalControl.isPaused = true;
    }
    public void HideMenu()
    {
        menu.gameObject.active = false;
        Time.timeScale = 1;
        GlobalControl.isPaused = false;
    }
}
