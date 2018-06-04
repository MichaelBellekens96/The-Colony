using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTurningDoor : MonoBehaviour {

    public float timeDoors;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    private Quaternion startPosLDoor;
    private Quaternion startPosRDoor;
    private Quaternion endPosLDoor = Quaternion.identity;
    private Quaternion endPosRDoor = Quaternion.identity;

    private Vector3 rotationL = new Vector3(0, 90, 0);
    private Vector3 rotationR = new Vector3(0, -90, 0);
    private float elapsedTime;
    private Coroutine coroutine;

    // Use this for initialization
    void Start()
    {
        LeftDoor = transform.Find("Left_door").gameObject;
        RightDoor = transform.Find("Right_door").gameObject;

        startPosLDoor = LeftDoor.transform.localRotation;
        startPosRDoor = RightDoor.transform.localRotation;

        endPosLDoor = Quaternion.Euler(startPosLDoor.eulerAngles + rotationL);
        endPosRDoor = Quaternion.Euler(startPosRDoor.eulerAngles + rotationR);
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

            LeftDoor.transform.localRotation = Quaternion.Lerp(startPosLDoor, endPosLDoor, elapsedTime);
            RightDoor.transform.localRotation = Quaternion.Lerp(startPosRDoor, endPosRDoor, elapsedTime);

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

            LeftDoor.transform.localRotation = Quaternion.Lerp(endPosLDoor, startPosLDoor, elapsedTime);
            RightDoor.transform.localRotation = Quaternion.Lerp(endPosRDoor, startPosRDoor, elapsedTime);

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
