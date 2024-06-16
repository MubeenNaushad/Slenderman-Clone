using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPages : MonoBehaviour
{
    public GameObject collectText;

    public AudioSource collectSound;

    private GameObject page;

    private bool inReach;

    private GameObject gameLogic;


    void Start()
    {
        collectText.SetActive(false);

        inReach = false;

        gameLogic = GameObject.FindWithTag("GameLogic");

        page = this.gameObject;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            collectText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            collectText.SetActive(false);
        }
    }

    void Update()
    {
        if(inReach && Input.GetButtonDown("pickup"))
        {
            gameLogic.GetComponent<GameLogic>().pageCount += 1;
            collectSound.Play();
            collectText.SetActive(false);
            page.SetActive(false);
            inReach = false;
        }

        
    }
}
