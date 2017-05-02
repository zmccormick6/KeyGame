using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleForeground : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "Foreground";
    }
}
