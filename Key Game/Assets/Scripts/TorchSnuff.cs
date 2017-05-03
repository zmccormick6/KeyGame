using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSnuff : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == GameObject.Find("TempPlayer").GetComponents<Collider2D>()[0])
        {
            gameObject.GetComponent<Animator>().SetInteger("Torch", 1);
        }
    }
}