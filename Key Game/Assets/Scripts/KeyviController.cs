using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyviController : MonoBehaviour
{
    public bool stop = false, inRange = false, dead = false, once = false;
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

    void FixedUpdate()
    {
        MoveBack();
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

    public void MoveBack()
    {
        if (dead == true)
        {
            inRange = true;
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-5, -1.5f), 0.25f);
        }
    }
}