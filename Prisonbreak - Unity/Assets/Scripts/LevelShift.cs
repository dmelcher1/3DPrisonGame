using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelShift : MonoBehaviour {

    public Animator animator;
    public PlayerController playerController;
    public GameObject player;
    public GameController gameController;
    private Animator playerAnimator;
    public Slider healthSlider;
    

    private void Start()
    {
        //playerController = GameObject.FindObjectOfType<PlayerController>();
        //gameController = GameObject.FindObjectOfType<GameController>();
        playerAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
		if((playerController.dead == true && playerController.fadeDelay <= 0.0f) || gameController.winnaCountDown <= 5.0f)
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
        healthSlider.value = playerController.health;
        playerController.spottedTimer = 10.0f;
        playerController.dead = false;
        playerController.caught = false;
        playerAnimator.SetBool("Caught", false);
        playerController.fadeDelay = 10.0f;
        animator.SetBool("FadeOut", false);
    }
}
