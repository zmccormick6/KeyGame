using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyviController : MonoBehaviour
{
    public bool stop = false, inRange = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
            GameObject.Find("Game Manager").GetComponent<TextController>().InGameKeyvi = GameObject.Find("Keyvi");
        }
        else
            inRange = false;
    }

    void Start()
    {
        //GameObject.Find("Game Manager").GetComponent<TextController>().InGameKeyvi = gameObject;
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