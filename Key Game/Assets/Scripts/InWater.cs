using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWater : MonoBehaviour
{
    public bool water = false;
    Collider2D SwordHitbox;

    void Start()
    {
        SwordHitbox = GameObject.Find("TempPlayer").GetComponents<Collider2D>()[0];
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other != SwordHitbox)
                water = true;
        }
        else
            water = false;
    }
}