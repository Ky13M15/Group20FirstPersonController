using System;
using UnityEngine;

public class DaynNite : MonoBehaviour
{
    public Material ourSkybox;

    //[Range(0f, 8.0f)]
    public float exposure;

    public float daysEnd = 24.0f;
    public float currentTime = 0.0f;
    public float timePassesBy = 0.1f; //This adds into current time

    public int daysCounter = 0; //This adds whenever current time equals 24
    public float negativeMultiplier = 25.0f;
    public float timeMultiplier = 15.0f;
    public TimeOfDay timeOfDay;
    
    //float changeTheTime;



   public Light ourSun;

    // public Color Red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RenderSettings.skybox = ourSkybox;
        //currentTime=

    }
    [Serializable]
    public enum TimeOfDay
    {
        Day,
        Nite
    }

    // Update is called once per frame
    void Update()
    {

        Days();
        
    }
    void Days()
    {
        exposure = Mathf.Clamp(exposure, 0.0f, 80.0f);
        ourSun.intensity = exposure;
        if (currentTime >= daysEnd)
        {
            daysCounter++;
            timePassesBy += 1.0f;
            Debug.Log("Days Gone");

            currentTime = 0.0f;

        }

        else if (currentTime != daysEnd)
        {
            ourSun.transform.Rotate(Vector3.up, timePassesBy * Time.deltaTime, Space.World);

             currentTime += timePassesBy * Time.deltaTime/*timeMultiplier*/;
            //ourSkybox.SetFloat("_Exposure", currentTime);
            Debug.Log("Current Time is: " + currentTime);

            if (currentTime <= 12)
            {
                //approaches the Day 
                exposure += timePassesBy * Time.deltaTime;
                timeOfDay = TimeOfDay.Day;
               
                ourSkybox.SetFloat("_Exposure", exposure);
                Debug.Log("<= 12: " + exposure);
                ourSun.intensity = exposure;
            }
            else if (currentTime >= 12)
            {
                //approaches night
                exposure -= timePassesBy * Time.deltaTime*negativeMultiplier;
                timeOfDay = TimeOfDay.Nite;

                ourSkybox.SetFloat("_Exposure", exposure);
                Debug.Log(">= 12: " + exposure);
                //Debug.Log("exposure:" + exposure);
                

            }
            Debug.Log("Days Going");
        }

        
    }
}
