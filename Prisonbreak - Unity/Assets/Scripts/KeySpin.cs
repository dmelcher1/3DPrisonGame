using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpin : MonoBehaviour {

    

	
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(0, 1, 0, Space.World);
	}
}
