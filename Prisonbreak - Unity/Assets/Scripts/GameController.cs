using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    private BoxCollider thisCol;
    public bool chicken = false;
    public float winnaCountDown = 30.0f;
    public Animator levelAnim;

	// Use this for initialization
	void Start ()
    {
        thisCol = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(chicken == true)
        {
            winnaCountDown -= 0.1f;
            if(winnaCountDown <= 0.0f)
            SceneManager.LoadScene("MoveTester");
        }
	}

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        chicken = true;
        thisCol.enabled = false;
    }
}
