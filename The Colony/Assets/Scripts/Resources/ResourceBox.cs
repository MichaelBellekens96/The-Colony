using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBox : MonoBehaviour {

    public ResourceData resourceData;
    public float resourceValue;

    public GameObject lid;
    public ResourceTypes type;

    private Material[] resourceboxMaterials;

    public void LoadData(ResourceData data)
    {
        resourceData = data;
        name = data.resourceName;
        resourceValue = data.value;
        type = resourceData.type;
        //lid.GetComponent<Renderer>().material.color = data.color;
        resourceboxMaterials = gameObject.GetComponent<Renderer>().materials;

        for (int i = 0; i < 2; i++)
        {
            if (resourceboxMaterials[i].name == "Lid (Instance)")
            {
                resourceboxMaterials[i].color = data.color;
            }
        }
    }
}
