using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrust : MonoBehaviour {

    Movement manager;
    bool grabed = false;
	// Use this for initialization
	void Start () {
        manager = GameObject.Find("knight").GetComponent<Movement>();
	}

    private void Update()
    {
        if (grabed && !Input.GetKeyDown(KeyCode.Space))
        {
            manager.gameObject.transform.position = manager.auxPos;
            manager.rb.velocity *= 0;
            manager.grabTime -= Time.deltaTime;
        }
        if (manager.grabTime <= 0)
        {
            ResetThrust();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == 10)
        {
            if(manager.grabTime > 0)
            {
                if (!grabed)
                {
                    manager.auxPos = manager.gameObject.transform.position;
                    grabed = true;
                    manager.grounded = true;
                }
                manager.thrustGrab = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        ResetThrust();
    }

    void ResetThrust()
    {
        manager.thrust.SetActive(false);
        manager.thrustGrab = false;
        grabed = false;
        manager.grounded = false;
        manager.grabTime = 3.0f;
        manager.t = 0.25f;
    }
}
