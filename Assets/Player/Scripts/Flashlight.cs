using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;

    public AudioSource turnOn;
    public AudioSource turnOff;

    private bool on;
    private bool off;




    void Start()
    {
        off = true;
        flashlight.SetActive(false);
    }




    void Update()
    {
        if (off && Input.GetButtonDown("flashlight"))
        {
            flashlight.SetActive(true);
            turnOn.Play();
            off = false;
            on = true;
        }
        else if (on && Input.GetButtonDown("flashlight"))
        {
            flashlight.SetActive(false);
            turnOff.Play();
            off = true;
            on = false;
        }



    }
}