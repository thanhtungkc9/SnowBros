using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInformation : MonoBehaviour {

    [SerializeField]
    Text DeathCount, Score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DeathCount.text = GlobalControl.DeathCount.ToString();
        Score.text = GlobalControl.Score.ToString();
	}
}
