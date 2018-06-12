using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioPlasticOven : MonoBehaviour {

    public bool ovenIsBaking;
    public bool BioPlasticReady = false;

    public float bakeTime;
    public float currentBakeTime;

    public GameObject RawFoodResourceBox;
    public Color RawFoodColor;
    public Color BioPlasticColor;

    public AudioSource ovenSound;

    public void StartOven()
    {
        ovenIsBaking = true;
        RawFoodResourceBox.GetComponent<Renderer>().materials[1].color = RawFoodColor;
        StartCoroutine(StartBakingRawFood());
    }

    private IEnumerator StartBakingRawFood()
    {
        // Show RawFood resourceBox
        RawFoodResourceBox.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        ovenSound.Play();

        // Slowly Lerp Raw Food color to BioPlastic color
        currentBakeTime = 0;
        while (currentBakeTime < bakeTime)
        {
            RawFoodResourceBox.GetComponent<Renderer>().materials[1].color = Color.Lerp(RawFoodColor, BioPlasticColor, Mathf.Clamp(currentBakeTime / bakeTime, 0, 1));
            currentBakeTime += Time.deltaTime;
            yield return null;
        }

        ovenSound.Stop();

        // Oven ready
        ovenIsBaking = false;
        BioPlasticReady = true;
    }

    public void GrabBioPlastic()
    {
        RawFoodResourceBox.GetComponent<Renderer>().materials[1].color = RawFoodColor;
        RawFoodResourceBox.SetActive(false);
        BioPlasticReady = false;
    }
}
