using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    public Camera buildCamera;
    private bool isColliding;
    public bool heightExceeding;

    private int numCollidingGameobjects = 0;
    // Update is called once per frame
    void FixedUpdate () {
        heightExceeding = Physics.Raycast(buildCamera.transform.position, Vector3.down, 49.4f, 1 << 9);
        Debug.DrawRay(buildCamera.transform.position, Vector3.down, Color.red);
	}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Layer: " + other.gameObject.layer.ToString());
        if (other.gameObject.layer == 10)
        {
            Debug.Log("Hitting a prop");
            numCollidingGameobjects++;
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
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
