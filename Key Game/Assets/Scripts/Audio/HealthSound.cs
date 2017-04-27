using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSound : MonoBehaviour
{
    public void PlayHealthSound()
    {
        GetComponent<AudioSource>().Play();
    }
}