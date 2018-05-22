﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSite : MonoBehaviour {

    public float buildPercentage;

    public GameObject linkedBuilding;
    public int numNeededMetal;
    public int numNeededBioPlastic;

    public int presentMetal = 0;
    public int presentBioPlastic = 0;

    public bool allResourcesPresent = false;
    public bool allowContruction = false;

    public List<Transform> presentMetalList = new List<Transform>();
    public List<Transform> presentBioPlasticList = new List<Transform>();

    public void InitializeConstructionSite(BuildingData data)
    {
        numNeededMetal = data.metal;
        numNeededBioPlastic = data.bioPlastic;
    }

    // Check if enough resources at site
    public void CheckResourcesForConstruction()
    {
        if (numNeededBioPlastic == presentBioPlastic && numNeededMetal == presentMetal)
        {
            allResourcesPresent = true;
        }
        else
        {
            allResourcesPresent = false;
        }
    }

    // Contruct building with welder
    public void ConstructBuilding(float value)
    {
        if (allResourcesPresent)
        {
            buildPercentage += value;
            if (buildPercentage >= 10)
            {
                PlaceBuilding();
            }
        }
    }

    // Destroy building site and enable appropriate building
    public void PlaceBuilding()
    {
        if (presentMetal > 0)
        {
            for (int i = 0; i < presentMetalList.Count; i++)
            {
                ResourceManager.Instance.RemoveResourceBox(presentMetalList[i]);
            }
            presentMetalList.Clear();
        }
        if (presentBioPlastic > 0)
        {
            for (int i = 0; i < presentBioPlasticList.Count; i++)
            {
                ResourceManager.Instance.RemoveResourceBox(presentBioPlasticList[i]);
            }
            presentBioPlasticList.Clear();
        }
        
        linkedBuilding.SetActive(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ResourceBox" && other.isTrigger == false)
        {
            if (other.GetComponent<ResourceBox>().type == ResourceTypes.Metal)
            {
                presentMetal++;
                presentMetalList.Add(other.transform);
            }
            if (other.GetComponent<ResourceBox>().type == ResourceTypes.BioPlastic)
            {
                presentBioPlastic++;
                presentBioPlasticList.Add(other.transform);
            }
            CheckResourcesForConstruction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ResourceBox" && other.isTrigger == false)
        {
            if (other.GetComponent<ResourceBox>().type == ResourceTypes.Metal)
            {
                presentMetal--;
                presentMetalList.Remove(other.transform);
            }
            if (other.GetComponent<ResourceBox>().type == ResourceTypes.BioPlastic)
            {
                presentBioPlastic--;
                presentBioPlasticList.Remove(other.transform);
            }
            CheckResourcesForConstruction();
        }
    }
}