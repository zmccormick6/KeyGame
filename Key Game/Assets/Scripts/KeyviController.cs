using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyviController : MonoBehaviour
{
    public bool stop = false, inRange = false;
    [SerializeField] private GameObject AButton;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (GameObject.Find("Game Manager").GetComponent<DoorSpawn>().enemyCount <= 0 || GameObject.Find("Level").GetComponent<LevelHold>().Level == 7)
            {
                inRange = true;
                GameObject.Find("Game Manager").GetComponent<TextController>().InGameKeyvi = GameObject.Find("Keyvi");
            }
        }
        else
        {
            inRange = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        inRange = false;
    }

    public void AButtoOn()
    {
        AButton.SetActive(true);
    }

    public void AButtonOff()
    {
        AButton.SetActive(false);
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