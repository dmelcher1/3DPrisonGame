using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private float smooth = 3.0f;
    private float doorAngleOpen = 105.0f;
    private float doorAngleClosed = 0.0f;
    public float timer;
    public bool opened = false;
    public bool inContact = false;
    private PlayerController pController;
    public Text keyText;

	// Use this for initialization
	void Start ()
    {
        pController = GameObject.FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= 0.1f;
        if (opened == true || timer > 0)
        {
            var target = Quaternion.Euler(0, doorAngleOpen, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, smooth * Time.deltaTime);
        }
        else if(timer <= 0)
        {
            var target = Quaternion.Euler(0, doorAngleClosed, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, smooth * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inContact = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(this.tag == "Locked" && pController.keysHeld > 0)
        {
            if (other.tag == "Player" && Input.GetButton("Interact") && inContact == true)
            {
                pController.keysHeld -= 1;
                keyText.text = "Keys: " + pController.keysHeld.ToString();
                this.gameObject.tag = "Unlocked";
                opened = true;
                timer = 20.0f;
                //this.gameObject.GetComponent<MeshRenderer>().material = unlockMat;
            }
        }
        else if(this.tag == "Unlocked")
        {
            if (other.tag == "Player" && Input.GetButton("Interact") && inContact == true)
            {
                opened = true;
                timer = 20.0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            inContact = false;
            opened = false;
        }
    }
}
