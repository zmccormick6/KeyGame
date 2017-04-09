using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyviController : MonoBehaviour
{
    public bool stop = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButton("Fire3"))
            {
                Talking();
            }
        }
    }

    public void Talking()
    {
        if (stop == false)
        {
            GameObject.Find("Game Manager").GetComponent<LevelSwitch>().pause = true;
            GameObject.Find("Game Manager").GetComponent<TextController>().StartTalking();
            stop = true;
        }
    }
}