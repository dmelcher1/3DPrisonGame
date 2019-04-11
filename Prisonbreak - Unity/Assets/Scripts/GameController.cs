using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    private BoxCollider thisCol;
    public bool chicken = false;
    public float winnaCountDown = 30.0f;
    public Animator levelAnim;
    public Text winText;

	// Use this for initialization
	void Start ()
    {
        thisCol = GetComponent<BoxCollider>();
        winText.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(chicken == true)
        {
            winnaCountDown -= 0.1f;
            winText.enabled = true;
            if(winnaCountDown <= 0.0f)
            SceneManager.LoadScene("MainMenu");
        }
	}

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        chicken = true;
        thisCol.enabled = false;
    }
}
