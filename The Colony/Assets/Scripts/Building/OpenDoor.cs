using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public float timeDoors;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    private Vector3 startPosLDoor;
    private Vector3 startPosRDoor;
    private Vector3 endPosLDoor = new Vector3(0.8f, 0, 0);
    private Vector3 endPosRDoor = new Vector3(-0.8f, 0, 0);

    private float elapsedTime;
    private Coroutine coroutine;

	// Use this for initialization
	void Start () {
        LeftDoor = transform.Find("Left_door").gameObject;
        RightDoor = transform.Find("Right_door").gameObject;

        startPosLDoor = LeftDoor.transform.localPosition;
        startPosRDoor = RightDoor.transform.localPosition;

        endPosLDoor += startPosLDoor;
        endPosRDoor += startPosRDoor;
	}

    private IEnumerator OpenDoors()
    {
        if (elapsedTime > 1)
        {
            elapsedTime = 0;
        }
        else if (elapsedTime < 1 && elapsedTime != 0)
        {
            elapsedTime = 1 - elapsedTime;
        }

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime / timeDoors;

            LeftDoor.transform.localPosition = Vector3.Lerp(startPosLDoor, endPosLDoor, elapsedTime);
            RightDoor.transform.localPosition = Vector3.Lerp(startPosRDoor, endPosRDoor, elapsedTime);

            yield return null;
        }
    }

    private IEnumerator CloseDoors()
    {
        if (elapsedTime > 1)
        {
            elapsedTime = 0;
        }
        else if (elapsedTime < 1 && elapsedTime != 0)
        {
            elapsedTime = 1 - elapsedTime;
        }

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime / timeDoors;

            LeftDoor.transform.localPosition = Vector3.Lerp(endPosLDoor, startPosLDoor, elapsedTime);
            RightDoor.transform.localPosition = Vector3.Lerp(endPosRDoor, startPosRDoor, elapsedTime);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            Debug.Log("Player detected near doors");
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(OpenDoors());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player detected near doors");
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(CloseDoors());
        }
    }
}