using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonRotation : MonoBehaviour {

    public float timeScale;

    public GameObject Moon;
    public GameObject Sun;
    public Light sunLight;

    private void Start()
    {
        //sunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
        Moon.transform.RotateAround(Vector3.zero, Vector3.right, timeScale * Time.deltaTime);
        Moon.transform.LookAt(Vector3.zero);

        Sun.transform.RotateAround(Vector3.zero, Vector3.right, timeScale * Time.deltaTime);
        Sun.transform.LookAt(Vector3.zero);
        if (Sun.transform.position.y < 0)
        {
            sunLight.intensity = 0;
        }
        else if (Sun.transform.position.y < 200)
        {
            sunLight.intensity = Mathf.Lerp(0, 1, Sun.transform.position.y / 200);
        }
        else
        {
            sunLight.intensity = 1;
        }
    }
}
