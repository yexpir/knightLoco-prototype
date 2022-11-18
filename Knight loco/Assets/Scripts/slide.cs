using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slide : MonoBehaviour {

    Movement manager;

	void Start () {
        manager = GameObject.Find("knight").GetComponent<Movement>();
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //print("holaaaaaaaaa");
        if(coll.gameObject.layer == 10)
        {
            manager.bouncing = true;
        }
    }
}
