using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShift : MonoBehaviour {

    public Animator animator;
    public PlayerController playerController;
    public GameObject player;
    

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
       

    }

    // Update is called once per frame
    void Update ()
    {
		if(playerController.dead == true && playerController.fadeDelay <= 0.0f)
        {
            FadeOnDeath();
        }
	}

    public void FadeOnDeath()
    {
        animator.SetBool("FadeOut", true);
    }

    public void FadeOver()
    {
        player.transform.position = playerController.playerStart.position;
        player.transform.rotation = playerController.playerStart.rotation;
        playerController.health = 3;
        playerController.spottedTimer = 10.0f;
        playerController.dead = false;
        playerController.caught = false;
        playerController.fadeDelay = 6.0f;
        animator.SetBool("FadeOut", false);
    }
}
