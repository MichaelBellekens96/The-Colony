using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

    public OpenDoor[] doors;
    public OpenTurningDoor[] rotatingDoors;
    public Light[] lights;

    public GameObject[] emissionMaterial;

    public BuildingData buildingData;

    public bool hasOxygen = false;
    public bool hasWater = false;
    public bool hasPower = false;
    public bool buildingEnabled = false;

    public Collider[] smallRocks;

    GameObject triggerObject;

    private void OnEnable()
    {
        if (!GetComponent<CollisionDetector>())
        {
            smallRocks = Physics.OverlapBox(transform.position, new Vector3(4, 4, 4), Quaternion.identity);
            for (int i = 0; i < smallRocks.Length; i++)
            {
                if (smallRocks[i].tag == "SmallRock")
                {
                    smallRocks[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerObject = other.gameObject;
        if (triggerObject.tag == "Player")
        {
            if (triggerObject.GetComponent<PlayerController>().insideBase)
            {
                StartCoroutine(CheckPlayetStillInBase());
            }
        }
    }

    private IEnumerator CheckPlayetStillInBase()
    {
        yield return new WaitForSeconds(0.3f);
        if (!triggerObject.GetComponent<PlayerController>().CheckStillInBase())
        {
            triggerObject.GetComponent<PlayerStats>().oxygenRate = triggerObject.GetComponent<PlayerTasks>().tempOxygenRate;
            triggerObject.GetComponent<PlayerStats>().Oxygen = 0;
            triggerObject.GetComponent<PlayerController>().insideBase = false;
            MainUIManager.Instance.UpdateStatsPanel();
        }

    }
}
