using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWater : MonoBehaviour
{
    public bool water = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            water = true;
        }
        else
            water = false;
    }
}