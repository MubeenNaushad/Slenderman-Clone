using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class GameLogic : MonoBehaviour
{
    public GameObject counter;
    public int pageCount;

    void Start()
    {
        pageCount = 0;
    }

    void Update()
    {
        // Use TextMeshProUGUI instead of Text
        counter.GetComponent<TextMeshProUGUI>().text = pageCount + "/8";
    }
}
