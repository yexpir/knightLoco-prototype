using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {

    Movement manager;

    private void Start()
    {
        manager = GameObject.Find("knight").GetComponent<Movement>();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        print(coll.gameObject.layer + "        " + manager.groundLayer);
        if (coll.gameObject.layer == 10)
        {
            manager.grounded = true;
            manager.jumping = false;
            print(manager.grounded);
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 10)
            manager.grounded = false;
    }
}
