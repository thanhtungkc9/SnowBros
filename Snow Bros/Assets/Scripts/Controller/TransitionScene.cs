using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TransitionScene : MonoBehaviour {
    public float timeSkip = 3.0f;
    private int a, b;           //Format     Stage + a + "-" + b                ex: a=1 b=2 -> Stage1-2

    public Text sceneName;
    public void Start()
    {
        a = (GlobalControl.CurrentScene-2) / 11 + 1;
        b = (GlobalControl.CurrentScene-1) % 11 ;
        sceneName.text = "Stage " + a.ToString() + "-" + b.ToString();
    }
    public void Update()
    {
        if (timeSkip > 0) timeSkip -= Time.deltaTime;
        else
        {
            SceneManager.LoadScene(GlobalControl.CurrentScene + 1);
        }
    }
}
