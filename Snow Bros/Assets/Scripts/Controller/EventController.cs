using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {

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
}
