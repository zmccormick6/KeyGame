using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<TempPlayerController>().Health = 6;
            other.gameObject.GetComponent<TempPlayerController>().AddHealth();
            Destroy(gameObject);
        }
    }
}