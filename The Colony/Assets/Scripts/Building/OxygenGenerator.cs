using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OxygenGenerator : MonoBehaviour {

    public float oxygenRadius;
    public BuildingController buildingController;

    public List<BuildingController> surroundingBuildings = new List<BuildingController>();
    private int baseLength;
    private List<GameObject> allBuildings;

    public AudioSource audioSource;
    private bool soundPlaying = false;

	// Use this for initialization
	void Start () {
        buildingController = GetComponent<BuildingController>();
        if (buildingController.buildingEnabled)
        {
            if (!soundPlaying)
            {
                audioSource.Play();
                soundPlaying = true;
            }
            allBuildings = BaseManager.Instance.BuildingList;
            baseLength = BaseManager.Instance.BuildingList.Count;
            for (int i = 0; i < baseLength; i++)
            {
                if (allBuildings[i].GetComponent<BuildingController>() && Vector3.Distance(transform.position, allBuildings[i].transform.position) < oxygenRadius)
                {
                    surroundingBuildings.Add(allBuildings[i].GetComponent<BuildingController>());
                }
            }
            for (int i = 0; i < surroundingBuildings.Count; i++)
            {
                surroundingBuildings[i].hasOxygen = true;
            }
        }
	}

    public void SpreadOxygen()
    {
        buildingController = GetComponent<BuildingController>();
        
        if (buildingController.buildingEnabled)
        {
            allBuildings = BaseManager.Instance.BuildingList;
            baseLength = BaseManager.Instance.BuildingList.Count;

            for (int i = 0; i < baseLength; i++)
            {
                if (allBuildings[i].GetComponent<BuildingController>() && Vector3.Distance(transform.position, allBuildings[i].transform.position) < oxygenRadius)
                {
                    surroundingBuildings.Add(allBuildings[i].GetComponent<BuildingController>());
                }
            }
            for (int i = 0; i < BaseManager.Instance.DisabledBuildings.Count; i++)
            {
                if (BaseManager.Instance.DisabledBuildings[i].GetComponent<BuildingController>() && Vector3.Distance(transform.position, BaseManager.Instance.DisabledBuildings[i].transform.position) < oxygenRadius)
                {
                    surroundingBuildings.Add(BaseManager.Instance.DisabledBuildings[i].GetComponent<BuildingController>());
                }
            }
            for (int i = 0; i < surroundingBuildings.Count; i++)
            {
                surroundingBuildings[i].hasOxygen = true;
            }
        }
    }

    public void PlaySounds()
    {
        soundPlaying = false;
        audioSource.Stop();
    }

    public void StopSounds()
    {
        soundPlaying = false;
        audioSource.Stop();
    }
}