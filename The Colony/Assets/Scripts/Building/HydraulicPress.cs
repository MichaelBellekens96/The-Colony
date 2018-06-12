using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPress : MonoBehaviour {

    public float productionTime;
    public bool processingMetalOre;
    public bool metalReady = false;

    public GameObject MetalOreBox;
    public GameObject MetalBox;

    public GameObject door;
    public GameObject press;

    public Vector3 doorOpen;
    public Vector3 doorClosed;

    public Vector3 pressClosed;
    public Vector3 pressOpen;

    public AudioSource up;
    public AudioSource down;

    public float currentProduction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            StartPress();
        }
    }

    public void StartPress()
    {
        MetalBox.SetActive(false);
        MetalOreBox.SetActive(false);
        processingMetalOre = true;
        metalReady = false;
        StartCoroutine(StartingPress());
    }

    private IEnumerator StartingPress()
    {
        // Enable metal ore resourcebox
        MetalOreBox.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // Close doors
        while (door.transform.localPosition != doorClosed)
        {
            door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, doorClosed, 1 * Time.deltaTime);
            yield return null;
        }

        // move press up and down for certain amount of time
        currentProduction = 0;
        while (currentProduction < productionTime)
        {
            down.Play();

            yield return new WaitForSeconds(0.6f);

            while (press.transform.localPosition != pressClosed)
            {
                press.transform.localPosition = Vector3.MoveTowards(press.transform.localPosition, pressClosed, 3 * Time.deltaTime);
                currentProduction += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            up.Play();

            while (press.transform.localPosition != pressOpen)
            {
                press.transform.localPosition = Vector3.MoveTowards(press.transform.localPosition, pressOpen, 0.4f * Time.deltaTime);
                currentProduction += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        // Change metal ore resourcebox to metal resourcebox
        MetalOreBox.SetActive(false);
        MetalBox.SetActive(true);

        // Open door
        while (door.transform.localPosition != doorOpen)
        {
            door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, doorOpen, 1 * Time.deltaTime);
            yield return null;
        }

        processingMetalOre = false;
        metalReady = true;
    }

    public void GrabMetalBox()
    {
        MetalBox.SetActive(false);
        metalReady = false;
    }
}
