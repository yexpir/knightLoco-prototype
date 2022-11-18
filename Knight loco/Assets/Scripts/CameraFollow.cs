using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float lerpAmount = 0.5f;
    public float offsetZ;
    public float offsetY;
    public float offsetX;
    Vector3 pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        pos = Vector3.Lerp(transform.position, player.transform.position, lerpAmount);
        pos.y += offsetY;
        pos.x += offsetX;
        pos.z = offsetZ;
        transform.position = pos;
	}
}
