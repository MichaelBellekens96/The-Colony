﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    private bool isColliding;
    public bool heightExceeding;

    private int numCollidingGameobjects = 0;
    private Vector3 rayPosition;

    // Update is called once per frame
    void FixedUpdate () {
        rayPosition = transform.position;
        rayPosition.y = 50;
        heightExceeding = Physics.Raycast(rayPosition, Vector3.down, 49.5f, 1 << 9);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.tag == "Player")
        {
            //Debug.Log("Hitting a prop");
            numCollidingGameobjects++;
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.tag == "Player")
        {
            numCollidingGameobjects--;
            if (numCollidingGameobjects == 0) isColliding = false;
        }
    }

    public bool CheckCollision()
    {
        if (isColliding || heightExceeding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}