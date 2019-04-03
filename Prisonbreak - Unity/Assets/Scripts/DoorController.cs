using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public Vector3 startPos;
    public Vector3 destination;
    public Vector3 currentPos;
    //private int touchProof = 0;
    private float smooth = 3.0f;
    private float totalDistance;
    public bool opened = false;
    public bool inContact = false;
    private float startTime;
    //public GameObject player;
    private PlayerController pController;
    public Material unlockMat;

	// Use this for initialization
	void Start ()
    {
        pController = GameObject.FindObjectOfType<PlayerController>();
        //pController = player.GetComponent<PlayerController>();
        startTime = Time.time;
        totalDistance = Vector3.Distance(startPos, destination);
        startPos = transform.position;
        currentPos = startPos;
        destination = new Vector3(startPos.x, startPos.y + 3.0f, startPos.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float distancePassed = (Time.time - startTime) * smooth;
        float journeyFraction = distancePassed / totalDistance;
	    if(opened == true && inContact == true)
        {
            transform.position = Vector3.MoveTowards(startPos, destination, journeyFraction);
            currentPos = transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(currentPos, startPos, journeyFraction);
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
            if (other.tag == "Player" && Input.GetButton("Interact") && startPos == transform.position)
            {
                pController.keysHeld -= 1;
                this.gameObject.tag = "Unlocked";
                opened = true;
                this.gameObject.GetComponent<MeshRenderer>().material = unlockMat;
            }
        }
        else if(this.tag == "Unlocked")
        {
            if (other.tag == "Player" && Input.GetButton("Interact") && startPos == transform.position)
            {
                opened = true;
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
