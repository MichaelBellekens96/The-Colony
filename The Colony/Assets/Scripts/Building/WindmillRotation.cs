using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillRotation : MonoBehaviour {

    public Vector3 rotationSpeed;

    private Rigidbody rb;
    private GameObject parentPart;
    private Quaternion deltaRotation;

    private void Start()
    {
        if (gameObject.GetComponentInParent<CollisionDetector>())
        {
            Destroy(gameObject.GetComponent<WindmillRotation>());
        }
        rb = this.gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        //rb.constraints = RigidbodyConstraints.FreezePosition;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void FixedUpdate () {
        //transform.Rotate(-transform.forward, speed * Time.deltaTime);
        deltaRotation = Quaternion.Euler(rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
	}
}
