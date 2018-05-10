using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBox : MonoBehaviour {

    public ResourceData resourceData;
    public float resourceValue;

    public GameObject lid;
    public ResourceTypes type;

    public void LoadData(ResourceData data)
    {
        resourceData = data;
        name = data.resourceName;
        resourceValue = data.value;
        type = resourceData.type;
        lid.GetComponent<Renderer>().material.color = data.color;
    }
}
