using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBox : MonoBehaviour {

    public ResourceData data;
    public float resourceValue;

    public GameObject lid;

	// Use this for initialization
	void Start () {
        name = data.resourceName;
        resourceValue = data.value;
        lid.GetComponent<Renderer>().material.color = data.color;

        data.PrintData();
	}
}
